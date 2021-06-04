using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerMoveData", menuName = "Data/Player/Player Movement")]
public class PlayerData : ScriptableObject
{
    [Header("Movement")]
    [SerializeField]
    private float _moveSpeed = 10f;

    [Header("Jumping")]
    [SerializeField]
    private bool _allowJump = true;
    [SerializeField]
    private float _jumpVelocity = 25;
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
    [SerializeField][Tooltip("Turn this to true if you don't want to hang while waiting for input")]
    private bool _shouldAutoClimb = false;
    [SerializeField]
    private float _ledgeClimbDuration = .5f;
    [SerializeField][Tooltip("Allow player to push downwards from ledge forcefully")]
    private float _ledgeDropPushVelocity = 10;
    [SerializeField]
    private Vector2 _startClimbOffset;
    [SerializeField]
    private Vector2 _stopClimbOffset;

    [Header("Dash")]
    [SerializeField]
    private bool _allowDash = true;
    [SerializeField]
    private float _dashCooldown = .5f;
    [SerializeField]
    private float _maxHoldTime = 1;
    [SerializeField][Range(0,1)]
    private float _holdTimeScale = .2f;
    [SerializeField]
    private float _dashDuration = 0.1f;
    [SerializeField]
    private float _dashVelocity = 50;
    [SerializeField]
    [Tooltip("makes the dash punchier, slows down while in air")]
    private float _dashDrag = 10;
    [SerializeField]
    [Range(0, 1)]
    [Tooltip("clamps upward movement at end of jump")]
    private float _dashEndYMultiplier = 0.2f;
    [SerializeField]
    private float _distanceBetweenAfterImages = 0.5f;

    [Header("Crouching")]
    [SerializeField]
    private float _crouchMoveVelocity = 3;
    [SerializeField]
    private float _crouchColliderHeight = 0.8f;
    [SerializeField]
    private float _standColliderHeight = 1.8f;

    [Header("Attack")]
    [SerializeField]
    private bool _allowAttack = true;
    [SerializeField]
    private float _attackCooldown = 0.5f;


    #region Getters
    // movement
    public float MoveSpeed => _moveSpeed;
    // jumping
    public bool AllowJump => _allowJump;
    public float JumpVelocity => _jumpVelocity;
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
    public bool ShouldAutoClimb => _shouldAutoClimb;
    public float LedgeClimbDuration => _ledgeClimbDuration;
    public float LedgeDropPushVelocity => _ledgeDropPushVelocity;
    public Vector2 StartClimbOffset => _startClimbOffset;
    public Vector2 StopClimbOffset => _stopClimbOffset;
    // dash
    public bool AllowDash => _allowDash;
    public float DashCooldown => _dashCooldown;
    public float MaxHoldTime => _maxHoldTime;
    public float HoldTimeScale => _holdTimeScale;
    public float DashDuration => _dashDuration;
    public float DashVelocity => _dashVelocity;
    public float DashDrag => _dashDrag;
    public float DashEndYMultiplier => _dashEndYMultiplier;
    public float DistanceBetweenAfterImages => _distanceBetweenAfterImages;
    // crouching
    public float CrouchMoveVelocity => _crouchMoveVelocity;
    public float CrouchColliderHeight => _crouchColliderHeight;
    public float StandColliderHeight => _standColliderHeight;
    // attack
    public bool AllowAttack => _allowAttack;
    public float AttackCooldown => _attackCooldown;
    #endregion
}
