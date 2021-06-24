using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
/// <summary>
/// Movement code for kinematic rigidbody built off from Unity Training module
/// the below is from Unity github example projects:
/// https://github.com/Unity-Technologies/PhysicsExamples2D/blob/2019.3/Assets/Scripts/SceneSpecific/Miscellaneous/KinematicTopDownController.cs
/// </summary>

[RequireComponent(typeof(Rigidbody2D))]
public class KinematicObject : MonoBehaviour
{
    public event Action ReceivedPush;

    [SerializeField]
    private Collider2D _collider;
    [SerializeField]
    private float _downMultiplier = 1.1f; // multiply down speed so downwards falling is faster than jump (if desired)
    [SerializeField]
    private float _minGroundNormalY = 0.65f;
    [SerializeField]
    private bool _useGravity = true;
    [SerializeField]
    private float _gravityScale = 1;

    protected Vector2 _groundNormal = new Vector2(0,1);
    protected const float _minMoveDistance = 0.001f;

    protected ContactFilter2D _contactFilter;
    protected RaycastHit2D[] _hitBuffer = new RaycastHit2D[16];
    protected const float _skinWidth = 0.01f;
    //protected List<RaycastHit2D> _colliderHits = new List<RaycastHit2D>(16);
    private Vector2 _velocity;
    protected Vector2 _requestedVelocity;

    public int FacingDirection { get; private set; } = 1;
    public Vector2 Velocity => _velocity;
    //public Vector2 Velocity => (_rb.position - PreviousPosition) / Time.fixedDeltaTime;
    public Vector2 Position => _rb.position;
    public float GravityScale => _gravityScale;
    public Vector2 PreviousPosition { get; private set; }
    public Rigidbody2D RB => _rb;
    public float InitialGravityScale { get; private set; }

    public bool IsGrounded { get; private set; }

    // pushing
    private Vector2 _pushVelocity;
    private Coroutine _pushRoutine;
    private Rigidbody2D _rb;

    // unorganized
    public int MaxIterations { get; private set; } = 2;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb.isKinematic = true;

        _contactFilter.useTriggers = false;
        _contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(_collider.gameObject.layer));
        _contactFilter.useLayerMask = true;

        // if we're not using gravity, enforce scale is 0
        if (!_useGravity)
            _gravityScale = 0;

        InitialGravityScale = _gravityScale;
    }

    private void FixedUpdate()
    {
        // clear previous queries
        IsGrounded = false;
        // save pre-calculated position for velocity calculations
        PreviousPosition = _rb.position;

        // gate for blocking new movement requests
        CalculateXVelocity();
        CalculateYVelocity();

        ApplyGravity();
        ApplyPushForce();   // force exerted externally upon this object, like knockback etc.

        // begin calculating new position
        Vector2 deltaPosition = _velocity * Time.fixedDeltaTime;

        // calculate the sloped movement angle, for moving reliably on slopes
        Vector2 moveAlongGround = new Vector2(_groundNormal.y, -_groundNormal.x);
        Vector2 move = moveAlongGround * deltaPosition.x;

        ApplyMovement(move, false);
        // then apply y movement
        move = Vector2.up * deltaPosition.y;
        ApplyMovement(move, true);

        // clear
        ClearRequestedMovement();
    }

    #region Public
    public void Move(Vector2 targetVelocity, bool allowFlip)
    {
        if (allowFlip)
            CheckIfShouldFlip(targetVelocity.x);
        _requestedVelocity += targetVelocity;
    }

    public void Move(float x, float y, bool allowFlip)
    {
        if (allowFlip)
            CheckIfShouldFlip(x);
        _requestedVelocity += new Vector2(x, y);
    }

    public void Move(float speed, Vector2 angle, int direction, bool allowFlip)
    {
        angle.Normalize();

        if (allowFlip)
            CheckIfShouldFlip(angle.x * direction);

        _requestedVelocity += new Vector2(angle.x * speed * direction, angle.y * speed);
    }

    public void MoveX(float x, bool allowFlip)
    {
        if (allowFlip)
            CheckIfShouldFlip(x);

        _requestedVelocity.x += x;
    }

    public void MoveY(float y)
    {
        _requestedVelocity.y += y;
    }

    public void Push(Vector2 direction, float strength, float duration)
    {
        direction.Normalize();

        StopPush();
        _pushRoutine = StartCoroutine(PushRoutine(direction * strength, duration));

        ReceivedPush?.Invoke();
    }

    public void StopPush()
    {
        if (_pushRoutine != null)
            StopCoroutine(_pushRoutine);
    }

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
        _pushVelocity.y = 0;
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
        //TODO check and correct for overlap here
        _rb.MovePosition(newPosition);
    }

    public void MovePositionInstant(Vector2 newPosition)
    {
        //TODO check and correct for overlap here
        _rb.position = newPosition;
        SetVelocityZero();
    }

    public void Flip()
    {
        FacingDirection *= -1;
        _rb.transform.Rotate(0, 180, 0);
    }

    public void CheckIfShouldFlip(float xDirection)
    {
        xDirection = Mathf.Clamp(xDirection, -1, 1);
        xDirection = Mathf.RoundToInt(xDirection);
        if (xDirection != 0 && xDirection != FacingDirection)
        {
            Flip();
        }
    }

    // remove this collider OUT of other collider
    public void MoveOutOfCollider(Collider2D overlappingCollider)
    {
        // if we're supposed to be ignoring this layer, don't do anything
        if (_contactFilter.IsFilteringLayerMask(overlappingCollider.gameObject))
        {
            return;
        }
        // calculate collider distance
        ColliderDistance2D colliderDistance =
            Physics2D.Distance(_collider, overlappingCollider);

        // if we're overlapped, remove it
        if (colliderDistance.isOverlapped)
        {
            _rb.position += colliderDistance.normal * ((colliderDistance.distance + _skinWidth));
        }
    }

    // remove other collider OUT of this one
    public void RemoveOverlappingCollider(Collider2D overlappingCollider)
    {
        // if we're supposed to be ignoring this layer, don't do anything
        if (_contactFilter.IsFilteringLayerMask(overlappingCollider.gameObject))
        {
            return;
        }
        // calculate collider distance
        ColliderDistance2D colliderDistance =
            Physics2D.Distance(_collider, overlappingCollider);

        // if we're overlapped, remove it
        if (colliderDistance.isOverlapped)
        {
            overlappingCollider.attachedRigidbody.position
                += colliderDistance.normal * ((colliderDistance.distance + _skinWidth));
        }
    }
    #endregion

    private void ApplyGravity()
    {
        Vector2 gravityForce;
        // add extra velocity if we're moving downwards
        if(_velocity.y < 0)
            gravityForce = _downMultiplier * _gravityScale * Physics2D.gravity * Time.fixedDeltaTime;
        else
            gravityForce = _gravityScale * Physics2D.gravity * Time.fixedDeltaTime;

        _velocity += gravityForce;
    }

    private void ClearRequestedMovement()
    {
        _requestedVelocity = Vector2.zero;
    }

    private void ApplyMovement(Vector2 move, bool isVertical)
    {
        // if we're moving at all substantially, check colliders
        float distance = move.magnitude;
        // if we're moving a significant amount
        if(distance > _minMoveDistance)
        {
            // find nearby colliders and store them
            int hitCount = _collider.Cast(move, _contactFilter, _hitBuffer, distance + _skinWidth);
            for (int i = 0; i < hitCount; i++)
            {
                Vector2 currentNormal = _hitBuffer[i].normal;
                // is surface flat enouh to land on?
                if (currentNormal.y > _minGroundNormalY)
                {
                    IsGrounded = true;
                    // if the normal fits within our slope, save it as our current ground normal
                    if (isVertical)
                    {
                        _groundNormal = currentNormal;
                        currentNormal.x = 0;
                    }
                }

                // if we're grounded, adjust velocity based on slope
                if (IsGrounded)
                {
                    // cut velocity based on our slope
                    float projection = Vector2.Dot(_velocity, currentNormal);
                    if (projection < 0)
                    {
                        // reduce the calculated amount from velocity
                        _velocity -= (projection * currentNormal);
                    }
                }
                // otherwise we're airborn, cancel vertical velocity if there's a ceiling
                else
                {
                    //_velocity.x = 0;
                    _velocity.y = Mathf.Min(_velocity.y, 0);
                }
                // if we're close to a collider, only move up to our skinwidth
                float modifiedDistance = _hitBuffer[i].distance - _skinWidth;
                if (modifiedDistance < distance)
                    distance = modifiedDistance;
            }
        }
        // move to new calculated position
        Vector2 newMoveDelta = move.normalized * distance;
        Vector2 newPosition = _rb.position + newMoveDelta;
        //Debug.Log("Delta Position: " + newMoveDelta);
        //Debug.Log("Target Position: " + newPosition);
        _rb.position = newPosition;
        // why doesn't the below work??? - I think it's because ApplyMovement is called twice in this query
        //_rb.MovePosition(newPosition);
    }

    private void CalculateXVelocity()
    {
        // we may want different velocity judgements depending on whether or not we're using gravity
        if (_useGravity)
        {
            _velocity.x = _requestedVelocity.x;
        }
        else
        {
            _velocity.x = _requestedVelocity.x;
        }
    }

    private void CalculateYVelocity()
    {
        // if we're using gravity, only change vertical if we have input, otherwise gravity will do its thing
        if(_useGravity)
        {
            if(_requestedVelocity.y != 0)
            {
                _velocity.y = _requestedVelocity.y;
            }
        }
        // otherwise, we're not using gravity just set y velocity to requested, including 0
        else
        {
            // don't do anything
            _velocity.y = _requestedVelocity.y;
        }
    }

    private IEnumerator PushRoutine(Vector2 pushForce, float duration)
    {
        _pushVelocity = pushForce;
        // because we're accounting for gravity, apply Y force once, initially
        if(_useGravity)
            MoveY(pushForce.y);

        for (float elapsedTime = 0; elapsedTime < duration; elapsedTime += Time.deltaTime)
        {
            // calculate
            Vector2 newPushForce = Vector2.Lerp
                (pushForce, Vector2.zero, elapsedTime / duration);
            _pushVelocity = newPushForce;

            yield return null;
        }
        _pushVelocity = Vector2.zero;
    }

    private void ApplyPushForce()
    {
        if(_pushVelocity.x != 0)
        {
            _velocity.x += _pushVelocity.x;
        }

        if(!_useGravity)
        {
            _velocity.y += _pushVelocity.y;
        }
    }
}
