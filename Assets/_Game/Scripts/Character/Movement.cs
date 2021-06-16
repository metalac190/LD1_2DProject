using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Movement code for kinematic rigidbody built off from Unity Training module
/// </summary>
public class Movement : MonoBehaviour
{
    [SerializeField]
    private Collider2D _collider;
    [SerializeField]
    private Rigidbody2D _rb;
    private float _gravityScale = 1;

    [SerializeField]
    private float _minGroundNormalY = 0.65f;

    protected bool _isGrounded;
    protected Vector2 _groundNormal;
    protected const float _minMoveDistance = 0.001f;

    protected ContactFilter2D _contactFilter;
    protected RaycastHit2D[] _hitBuffer = new RaycastHit2D[16];
    protected const float _skinWidth = 0.01f;
    protected List<RaycastHit2D> _hitBufferList = new List<RaycastHit2D>(16);
    private Vector2 _velocity;
    protected Vector2 _requestedVelocity;

    public int FacingDirection { get; private set; } = 1;
    public Vector2 Velocity => _velocity;
    public Vector2 Position => _rb.position;
    public float GravityScale => _gravityScale;

    public bool IsUsingGravity { get; private set; } = true;
    public bool IsInputLocked { get; private set; } = false;

    // use these booleans do differentiate between 'move 0' and 'no movement requested, default to 0'
    private bool _xMoveRequested = false;
    private bool _yMoveRequested = false;

    private void Awake()
    {
        _contactFilter.useTriggers = false;
        _contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(_collider.gameObject.layer));
        _contactFilter.useLayerMask = true;
    }

    private void FixedUpdate()
    {
        // clear
        _isGrounded = false;

        // gate for blocking new movement requests
        CalculateXVelocity();
        CalculateYVelocity();
        CheckIfShouldFlip();

        Vector2 gravityForce = _gravityScale * Physics2D.gravity * Time.fixedDeltaTime;
        _velocity += gravityForce;

        // begin calculating new position
        Vector2 deltaPosition = _velocity * Time.deltaTime;
        // calculate the sloped movement angle, for moving reliably on slops
        Vector2 moveAlongGround = new Vector2(_groundNormal.y, -_groundNormal.x);
        // apply x movement first
        Vector2 move = moveAlongGround * deltaPosition.x;
        ApplyMovement(move, false);

        // then apply y movement
        move = Vector2.up * deltaPosition.y;
        ApplyMovement(move, true);

        // clear
        ClearRequestedMovement();
}

    private void ClearRequestedMovement()
    {
        _requestedVelocity = Vector2.zero;
        _xMoveRequested = false;
        _yMoveRequested = false;
    }

    public void Move(Vector2 targetVelocity)
    {
        _requestedVelocity += targetVelocity;
        _xMoveRequested = true;
        _yMoveRequested = true;
    }

    public void Move(float x, float y)
    {
        _requestedVelocity += new Vector2(x, y);
        _xMoveRequested = true;
        _yMoveRequested = true;
    }

    public void Move(float velocity, Vector2 angle, int direction)
    {
        angle.Normalize();
        _requestedVelocity += new Vector2(angle.x * velocity * direction, angle.y * velocity);
        _xMoveRequested = true;
        _yMoveRequested = true;
    }

    public void MoveX(float x)
    {
        _requestedVelocity.x += x;
        _xMoveRequested = true;
    }

    public void MoveY(float y)
    {
        _requestedVelocity.y += y;
        _yMoveRequested = true;
    }

    private void ApplyMovement(Vector2 move, bool yMovement)
    {
        // if we're moving at all substantially, check colliders
        float distance = move.magnitude;
        if(distance > _minMoveDistance)
        {
            // find nearby colliders and store them
            int hitCount = _collider.Cast(move, _contactFilter, _hitBuffer, distance + _skinWidth);
            _hitBufferList.Clear();
            for (int i = 0; i < hitCount; i++)
            {
                _hitBufferList.Add(_hitBuffer[i]);
            }
            // search throuh colliders
            for (int i = 0; i < _hitBufferList.Count; i++)
            {
                Vector2 currentNormal = _hitBufferList[i].normal;
                // mark if collider is below us (ground)
                if (currentNormal.y > _minGroundNormalY)
                {
                    _isGrounded = true;
                    // if the normal fits within our slope, save it as our current ground normal
                    if (yMovement)
                    {
                        _groundNormal = currentNormal;
                        currentNormal.x = 0;
                    }
                }
                // cut velocity based on our slope
                float projection = Vector2.Dot(_velocity, currentNormal);
                if(projection < 0)
                {
                    // reduce the calculated amount from velocity
                    _velocity -= (projection * currentNormal);
                }
                // if we're close to a collider, only move up to our skinwidth
                float modifiedDistance = _hitBufferList[i].distance - _skinWidth;
                if(modifiedDistance < distance)
                {
                    distance = modifiedDistance;
                }
            }
        }
        // move to new calculated position
        _rb.position += (move.normalized * distance);
        //_rb.MovePosition(_rb.position + (move.normalized * distance));
    }

    private void CalculateXVelocity()
    {
        if (_xMoveRequested)
        {
            _velocity.x = _requestedVelocity.x;
        }
    }

    private void CalculateYVelocity()
    {
        // if we have y move request, apply it, otherwise just use gravity
        if (_yMoveRequested)
        {
            _velocity.y = _requestedVelocity.y;
        }
    }


    #region Old Code



    public void HoldPosition(Vector2 position)
    {
        SetVelocityZero();
        // this seems redundant, but physics needs to explicitly be told that RB has stopped
        // moving or some systems (like Cinemachine) don't follow properly
        //_rb.MovePosition(position);
        _rb.position = position;
    }

    public void SetGravityScale(float gravityScale)
    {
        //_velocity += gravityForce * Physics2D.gravity * Time.deltaTime;
        _gravityScale = gravityScale;
    }

    public void SetVelocityXZero()
    {
        _velocity.x = 0;
        _requestedVelocity.x = 0;
    }

    public void SetVelocityYZero()
    {
        _velocity.y = 0;
        _requestedVelocity.y = 0;
    }

    public void SetVelocityZero()
    {
        _velocity = Vector2.zero;
        _requestedVelocity = Vector2.zero;
    }

    public void SetVelocity(float xVelocity, float yVelocity)
    {
        _velocity = new Vector2(xVelocity, yVelocity);
        _requestedVelocity = new Vector2(xVelocity, yVelocity);
    }

    // moves to new position, accounting for path between points
    public void MovePositionSmooth(Vector2 newPosition)
    {
        _rb.MovePosition(newPosition);
    }

    public void MovePositionInstant(Vector2 newPosition)
    {
        _rb.position = newPosition;
    }

    public void Flip()
    {
        FacingDirection *= -1;
        _rb.transform.Rotate(0, 180, 0);
    }

    public void CheckIfShouldFlip()
    {  
        int xDirection = Mathf.RoundToInt(Velocity.x);

        xDirection = Mathf.Clamp(xDirection, -1, 1);
        if (xDirection != 0 && xDirection != FacingDirection)
        {
            Flip();
        }
    }

    #endregion
}
