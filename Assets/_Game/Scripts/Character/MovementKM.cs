using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
/// <summary>
/// Movement code for kinematic rigidbody built off from Unity Training module
/// the below is from Unity github example projects:
/// https://github.com/Unity-Technologies/PhysicsExamples2D/blob/2019.3/Assets/Scripts/SceneSpecific/Miscellaneous/KinematicTopDownController.cs
/// </summary>
public class MovementKM : MonoBehaviour
{
    public event Action ReceivedPush;

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
    [SerializeField]
    protected ContactFilter2D _contactFilter;
    protected RaycastHit2D[] _hitBuffer = new RaycastHit2D[16];
    protected const float _skinWidth = 0.01f;
    protected List<RaycastHit2D> _colliderHits = new List<RaycastHit2D>(16);
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

    // pushing
    private Vector2 _pushVelocity;
    private Coroutine _pushRoutine;

    // unorganized
    public int MaxIterations { get; private set; } = 2;

    private float _gravityBuildup = 0;

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

        ApplyGravity();
        ApplyPushForce();   // force exerted externally upon this object, like knockback etc.

        // begin calculating new position
        Vector2 deltaPosition = _velocity * Time.fixedDeltaTime;
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

    private void ApplyGravity()
    {
        Vector2 gravityForce = _gravityScale * Physics2D.gravity * Time.fixedDeltaTime;
        _velocity += gravityForce;
    }

    private void ClearRequestedMovement()
    {
        _requestedVelocity = Vector2.zero;
    }

    public void Move(Vector2 targetVelocity)
    {
        _requestedVelocity += targetVelocity;
    }

    public void Move(float x, float y)
    {
        _requestedVelocity += new Vector2(x, y);
    }

    public void Move(float speed, Vector2 angle, int direction)
    {
        angle.Normalize();
        _requestedVelocity += new Vector2(angle.x * speed * direction, angle.y * speed);
    }

    public void MoveX(float x)
    {
        _requestedVelocity.x += x;
    }

    public void MoveY(float y)
    {
        _requestedVelocity.y += y;
    }

    private void ApplyMovement(Vector2 move, bool yMovement)
    {
        // if we're moving at all substantially, check colliders
        float distance = move.magnitude;
        if(distance > _minMoveDistance)
        {
            // find nearby colliders and store them
            int hitCount = _collider.Cast(move, _contactFilter, _hitBuffer, distance + _skinWidth);
            _colliderHits.Clear();
            for (int i = 0; i < hitCount; i++)
            {
                _colliderHits.Add(_hitBuffer[i]);
            }
            // search throuh colliders
            for (int i = 0; i < _colliderHits.Count; i++)
            {
                Vector2 currentNormal = _colliderHits[i].normal;
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
                float modifiedDistance = _colliderHits[i].distance - _skinWidth;
                if(modifiedDistance < distance)
                {
                    distance = modifiedDistance;
                }
            }
        }
        // move to new calculated position
        Vector2 newMoveDelta = move.normalized * distance;
        Vector2 newPosition = _rb.position + newMoveDelta;
        //Debug.Log("Delta Position: " + newMoveDelta);
        //Debug.Log("Target Position: " + newPosition);
        _rb.position = newPosition;
        // why doesn't the below work???
        //_rb.MovePosition(newPosition);
    }

    private void CalculateXVelocity()
    {
        _velocity.x = _requestedVelocity.x;
    }

    private void CalculateYVelocity()
    {
        //TODO this is messy, we should calculate increasing gravity independent of input request reliability
        if(_requestedVelocity.y != 0)
        {
            _velocity.y = _requestedVelocity.y;
        }
        
    }

    public void Push(Vector2 direction, float strength, float duration)
    {
        Debug.Log("PUSH");

        direction.Normalize();

        if (_pushRoutine != null)
            StopCoroutine(_pushRoutine);
        _pushRoutine = StartCoroutine(PushRoutine(direction * strength, duration));

        ReceivedPush?.Invoke();
    }

    private IEnumerator PushRoutine(Vector2 pushForce, float duration)
    {
        _pushVelocity = pushForce;
        // because we're accounting for gravity, apply Y force once, initially
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

    public void RemoveOverlap(Collider2D overlappingCollider, float extraPushMultiplier)
    {
        if(extraPushMultiplier < 1)
        {
            extraPushMultiplier = 1;
        }

        // if we're supposed to be ignoring this layer, don't do anything
        if (_contactFilter.IsFilteringLayerMask(overlappingCollider.gameObject))
        {
            Debug.Log("Filtering, don't do anything");
            return;
        }
        // calculate collider distance
        ColliderDistance2D colliderDistance = 
            Physics2D.Distance(_collider, overlappingCollider);

        // if we're overlapped, remove it
        if (colliderDistance.isOverlapped)
        {
            //Move(colliderDistance.normal * (colliderDistance.distance));
            // use contact offset
            //ContactPoint2D contact = Physics2D.GetContacts(_collider, otherCollision.collider);
            // use extra push Multiplier to push object out further. TODO: do this more accurately later
            _rb.position += colliderDistance.normal * (colliderDistance.distance * extraPushMultiplier);
        }
    }

    // below are other approaches to KM movement. Still determining best method for resuable controllers
    /*
    public void ApplyMove()
    {
        // arbitrarily small number for calculations
        const float Epsilon = 0.005f;

        // don't perform work if the movement is small enough
        if (_velocity.sqrMagnitude <= Epsilon)
            return;
        // get move direction
        Vector2 moveDirection = _velocity.normalized;
        // calculate distance to cover this cycle
        float distanceRemaining = _velocity.magnitude * Time.fixedDeltaTime;
        // this can be increased if we have more complex geometry, but 2-3 is fine
        int maxIterations = MaxIterations;

        // save original position, since we will be moving it around during query
        Vector2 startPosition = _rb.position;
        // iterate up to cap OR until we have no more distance
        while (maxIterations-- > 0 &&
            distanceRemaining > Epsilon &&
            moveDirection.sqrMagnitude > Epsilon)
        {
            float distance = distanceRemaining;
            // perform cast in the move direction using RB colliders
            int hitCount = _rb.Cast(moveDirection, _contactFilter, _colliderHits, distance);
            // did we have any hits?
            if(hitCount > 0)
            {
                // for now we're only interested in the first thing we hit
                var hit = _colliderHits[0];
                // only move if we're beyond the collider 'skin buffer' area
                if(hit.distance > _skinWidth)
                {
                    // calculate taret distance
                    distance = hit.distance - _skinWidth;
                    // reposition RB temporarily to continue iterations
                    _rb.position += moveDirection * distance;
                }
                else
                {
                    // we had a hit but it resulted in overlap
                    distance = 0;
                }
                // clamp movement direction
                // NOT this is how we iterate and chaange direction for queries
                moveDirection -= hit.normal * Vector2.Dot(moveDirection, hit.normal);
            }
            // no hits, so move the whole distance
            else
            {
                _rb.position += moveDirection * distance;
            }
            // remove tested distance from remaining and continue tests
            distanceRemaining -= distance;
        };

        // save and move rb back to original position before queries
        // NOTE: this can be avoided with different query types (sphere casts, etc.)
        Vector2 targetPosition = _rb.position;
        _rb.position = startPosition;

        // FINALLY move the RB to target position now that calculations are complete
        _rb.MovePosition(targetPosition);
    }
    */
}
