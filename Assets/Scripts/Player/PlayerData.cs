using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Data/Player")]
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
    [SerializeField]
    private float _ledgeClimbDuration = .5f;
    [SerializeField][Tooltip("Allow player to push downwards from ledge forcefully")]
    private float _ledgeDropPushVelocity = 10;
    [SerializeField]
    private Vector2 _startClimbOffset;
    [SerializeField]
    private Vector2 _stopClimbOffset;

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
    public float LedgeClimbDuration => _ledgeClimbDuration;
    public float LedgeDropPushVelocity => _ledgeDropPushVelocity;
    public Vector2 StartClimbOffset => _startClimbOffset;
    public Vector2 StopClimbOffset => _stopClimbOffset;
    #endregion
}
