using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerMoveData", menuName = "Data/Player/Player Movement")]
public class PlayerMoveData : ScriptableObject
{
    [Header("Movement")]
    [SerializeField]
    private float _moveSpeed = 10f;
    [SerializeField]
    private float _groundAccelToMax = .2f;
    [SerializeField]
    private float _groundDecelToZero = .1f;
    //[SerializeField]
    //private float _gravityScale = 6;

    [Header("Jumping")]
    [SerializeField]
    private bool _allowJump = true;
    [SerializeField]
    private float _jumpVelocity = 25;
    [SerializeField][Tooltip("Guarantees jump state briefly before going to fall")]
    private float _minJumpTime = .1f;
    [SerializeField][Range(0,1)][Tooltip("Short jump height compared to full jump")]
    private float _shortJumpHeightScale = 0.5f;
    [SerializeField][Tooltip("Allow a brief buffer for jumping right after falling")]
    private float _jumpAfterFallDuration = 0.1f;

    [Header("Air Jump")]
    [SerializeField]
    private int _amountOfAirJumps = 1;
    [SerializeField]
    private float _airJumpVelocity = 20;
    [SerializeField][Range(0, 1)][Tooltip("Short jump height compared to full air jump")]
    private float _shortAirJumpHeightScale = 0.5f;

    [Header("Wall Jump")]
    [SerializeField]
    private bool _allowWallJump = false;
    [SerializeField]
    private float _wallJumpVelocity = 20;
    [SerializeField]
    private Vector2 _wallJumpAngle = new Vector2(1, 2);
    [SerializeField][Range(0,1)][Tooltip("This dampens movement a bit during the wall jump")]
    private float _wallJumpMovementDampener = .5f;
    [SerializeField][Tooltip("Briefly locks wall jump input to prevent single wall cheese")]
    private float _moveInputLockDuration = .35f;
    [SerializeField][Tooltip("Allow a brief buffer for walljumping right after leaving the wall")]
    private float _wallJumpAfterFallDuration = 0.1f;

    [Header("Falling")]
    [Tooltip("Time to complete land animation")]
    [SerializeField]
    private float _landDuration = .2f;

    [Header("Wall Settings")]
    [SerializeField]
    private bool _allowWallSlide = false;
    [SerializeField]
    private float _wallSlideVelocity = 0.2f;
    [SerializeField][Tooltip("This value gets added every physics update to increase the slide velocity over time")]
    private float _wallSlideAcceleration = 0.2f;
    [SerializeField]
    private bool _allowWallGrab = false;
    [SerializeField]
    private float _wallClimbVelocity = 3;
    [SerializeField]
    private bool _allowWallClimb = false;

    [Header("Ledge")]
    [SerializeField]
    private bool _allowLedgeHop = false;
    [SerializeField]
    private float _ledgeHopAmount = 20;
    [SerializeField]
    private float _ledgeHopDuration = .25f;

    [Header("Dash")]
    [SerializeField]
    private bool _allowDash = true;
    [SerializeField]
    private bool _allowDirectionInput = false;
    [SerializeField]
    private float _dashCooldown = 1;
    [SerializeField]
    private float _dashDuration = 0.1f;
    [SerializeField]
    private float _dashVelocity = 50;
    [SerializeField][Range(0,1)]
    [Tooltip("makes the dash punchier, slows down while in air")]
    private float _dashingGravityScale = 0;
    [SerializeField]
    [Range(0, 1)]
    [Tooltip("clamps upward movement at end of dash")]
    private float _dashEndScale = 0.2f;

    [Header("Crouching")]
    [SerializeField]
    private bool _allowCrouch = false;
    [SerializeField]
    private float _crouchMoveVelocity = 3;
    [SerializeField]
    private float _crouchColliderHeight = 0.8f;
    [SerializeField]
    private float _standColliderHeight = 1.8f;

    [Header("Attack")]
    [SerializeField]
    private bool _allowAttack = true;
    [SerializeField][Range(0, 1)]
    [Tooltip("Cuts velocity by this ratio whenever an air attack occurs")]
    private float _airAttackVelocityYDampen = .5f;
    [SerializeField]
    private float _bounceAttackVertical = 25;


    #region Getters
    // movement
    public float MoveSpeed => _moveSpeed;
    public float GroundAccelToMax => _groundAccelToMax;
    public float GroundDecelToZero => _groundDecelToZero;
    //public float GravityScale => _gravityScale;
    // jumping
    public bool AllowJump => _allowJump;
    public float JumpVelocity => _jumpVelocity;
    public float MinJumpTime => _minJumpTime;
    public float ShortJumpHeightScale => _shortJumpHeightScale;
    public float JumpAfterFallDuration => _jumpAfterFallDuration;
    // air jump
    public int AmountOfAirJumps => _amountOfAirJumps;
    public float AirJumpVelocity => _airJumpVelocity;
    public float ShortAirJumpHeightScale => _shortAirJumpHeightScale;
    // wall jump
    public bool AllowWallJump => _allowWallJump;
    public float WallJumpVelocity => _wallJumpVelocity;
    public Vector2 WallJumpAngle => _wallJumpAngle;
    public float WallJumpMovementDampener => _wallJumpMovementDampener;
    public float MoveInputLockDuration => _moveInputLockDuration;
    public float WallJumpAfterFallDuration => _wallJumpAfterFallDuration;
    // landing
    public float LandDuration => _landDuration;
    // wall
    public bool AllowWallSlide => _allowWallSlide;
    public float WallSlideVelocity => _wallSlideVelocity;
    public float WallSlideAcceleration => _wallSlideAcceleration;
    public bool AllowWallGrab => _allowWallGrab;
    public bool AllowWallClimb => _allowWallClimb;
    public float WallClimbVelocity => _wallClimbVelocity;
    // ledge
    public bool AllowLedgeHop => _allowLedgeHop;
    public float LedgeHopAmount => _ledgeHopAmount;
    public float LedgeHopDuration => _ledgeHopDuration;
    // dash
    public bool AllowDash => _allowDash;
    public bool AllowDirectionInput => _allowDirectionInput;
    public float DashCooldown => _dashCooldown;
    public float DashDuration => _dashDuration;
    public float DashVelocity => _dashVelocity;
    public float DashingGravityScale => _dashingGravityScale;
    public float DashEndScale => _dashEndScale;

    // crouching
    public bool AllowCrouch => _allowCrouch;
    public float CrouchMoveVelocity => _crouchMoveVelocity;
    public float CrouchColliderHeight => _crouchColliderHeight;
    public float StandColliderHeight => _standColliderHeight;
    // attack
    public bool AllowAttack => _allowAttack;
    public float AirAttackVelocityYDampen => _airAttackVelocityYDampen;
    public float BounceAttackVertical => _bounceAttackVertical;
    #endregion
}
