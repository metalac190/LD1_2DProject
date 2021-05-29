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
    private float _jumpForce = 25;
    [SerializeField]
    private int _amountOfJumps = 1;
    [SerializeField][Range(0,1)][Tooltip("Short jump height compared to full jump")]
    private float _shortJumpHeightMultiplier = 0.5f;
    [SerializeField][Tooltip("Allow a brief buffer for jumping right after falling")]
    private float _jumpAfterFallDuration = 0.1f;
    
    [Header("Falling")]
    [Tooltip("Time to complete land animation")]
    [SerializeField]
    private float _landDuration = .2f;

    [Header("Wall Grab")]
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

    public float MoveSpeed => _moveSpeed;

    public float JumpForce => _jumpForce;
    public int AmountOfJumps => _amountOfJumps;
    public float ShortJumpHeightMultiplier => _shortJumpHeightMultiplier;
    public float JumpAfterFallDuration => _jumpAfterFallDuration;

    public float LandDuration => _landDuration;

    public bool AllowWallSlide => _allowWallSlide;
    public float WallSlideVelocity => _wallSlideVelocity;
    public float WallSlideAcceleration => _wallSlideAcceleration;
    public bool AllowWallGrab => _allowWallGrab;
    public bool AllowWallClimb => _allowWallClimb;
    public float WallClimbVelocity => _wallClimbVelocity;
}
