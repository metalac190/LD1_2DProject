using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SoundSystem;

[CreateAssetMenu(fileName = "PlayerSFX", menuName = "Data/Player/Player SFX")]
public class PlayerSFXData : ScriptableObject
{
    [Header("Player Sound Effects")]
    [SerializeField]
    private SFXOneShot _jumpSFX;
    [SerializeField]
    private SFXOneShot _airJumpSFX;
    [SerializeField]
    private SFXOneShot _wallJumpSFX;
    [SerializeField]
    private SFXOneShot _dashHoldSFX;
    [SerializeField]
    private SFXOneShot _dashReleaseSFX;
    [SerializeField]
    private SFXOneShot _ledgeCatchSFX;
    [SerializeField]
    private SFXOneShot _landSFX;
    [SerializeField]
    private SFXOneShot _wallGrabSFX;

    #region Getters
    // sounds
    public SFXOneShot JumpSFX => _jumpSFX;
    public SFXOneShot AirJumpSFX => _airJumpSFX;
    public SFXOneShot WallJumpSFX => _wallJumpSFX;
    public SFXOneShot DashHoldSFX => _dashHoldSFX;
    public SFXOneShot DashReleaseSFX => _dashReleaseSFX;
    public SFXOneShot LedgeCatchSFX => _ledgeCatchSFX;
    public SFXOneShot LandSFX => _landSFX;
    public SFXOneShot WallGrabSFX => _wallGrabSFX;
    #endregion
}
