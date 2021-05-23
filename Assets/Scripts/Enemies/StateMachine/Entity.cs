using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public int FacingDirection { get; private set; } = 1;

    public FiniteStateMachine StateMachine { get; private set; }
    public EntityData EntityData => _entityData;
    public Rigidbody2D RB { get; private set; }
    public Animator Anim { get; private set; }

    [SerializeField]
    private EntityData _entityData;
    [SerializeField] 
    private Transform _wallCheck;
    [SerializeField] 
    private Transform _ledgeCheck;

    private Vector2 _velocityWorkspace;

    public virtual void Awake()
    {
        RB = GetComponent<Rigidbody2D>();
        Anim = GetComponent<Animator>();

        StateMachine = new FiniteStateMachine();

        FacingDirection = 1;
    }

    public virtual void Update()
    {
        StateMachine.CurrentState.LogicUpdate();
    }

    public virtual void FixedUpdate()
    {
        StateMachine.CurrentState.PhysicsUpdate();
    }

    public virtual void SetVelocity(float velocity)
    {
        _velocityWorkspace.Set(FacingDirection * velocity, RB.velocity.y);
        RB.velocity = _velocityWorkspace;
    }

    public virtual bool CheckWall()
    {
        return Physics2D.Raycast(_wallCheck.position, gameObject.transform.right, 
            _entityData.WallCheckDistance, _entityData.WhatIsGround);
    }

    public virtual bool CheckLedge()
    {
        return Physics2D.Raycast(_ledgeCheck.position, Vector2.down,
            _entityData.LedgeCheckDistance, _entityData.WhatIsGround);
    }

    public virtual void Flip()
    {
        FacingDirection *= -1;
        transform.Rotate(0, 180, 0);
    }

    public virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(_wallCheck.position, _wallCheck.position + 
            (Vector3)(Vector2.right * FacingDirection * _entityData.WallCheckDistance));
        Gizmos.DrawLine(_ledgeCheck.position, _ledgeCheck.position + 
            (Vector3)(Vector2.down * _entityData.LedgeCheckDistance));
    }
}
