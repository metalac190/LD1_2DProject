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
    private float _jumpVelocity = 25;
    [SerializeField]
    private int _amountOfJumps = 1;
    [SerializeField][Range(0,1)][Tooltip("Short jump height compared to full jump")]
    private float _shortJumpHeightMultiplier = 0.5f;
    [SerializeField][Tooltip("Allow a brief buffer for jumping right after falling")]
    private float _jumpAfterFallDuration = 0.1f;

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

    #region Getters
    // movement
    public float MoveSpeed => _moveSpeed;
    // jumping
    public float JumpVelocity => _jumpVelocity;
    public int AmountOfJumps => _amountOfJumps;
    public float ShortJumpHeightMultiplier => _shortJumpHeightMultiplier;
    public float JumpAfterFallDuration => _jumpAfterFallDuration;
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
    #endregion
}
