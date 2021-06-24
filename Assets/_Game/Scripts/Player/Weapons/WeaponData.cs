using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SoundSystem;

[CreateAssetMenu(fileName = "SO_WeaponData_", menuName = "Data/Weapon/Melee Weapon")]
public class WeaponData : ScriptableObject
{
    [SerializeField]
    private int _maxComboCount = 3;

    [SerializeField]
    private SFXOneShot _hitSFX;
    [SerializeField]
    private SFXOneShot _finisherSFX;

    [SerializeField]
    private MeleeAttack _groundAttack = new MeleeAttack();
    [SerializeField]
    private MeleeAttack _groundFinisher = new MeleeAttack();
    [SerializeField]
    private MeleeAttack _airAttack = new MeleeAttack();
    [SerializeField]
    private MeleeAttack _airFinisher = new MeleeAttack();
    [SerializeField]
    private MeleeAttack _wallAttack = new MeleeAttack();
    [SerializeField]
    private MeleeAttack _wallFinisher = new MeleeAttack();
    [SerializeField]
    private MeleeAttack _bounceAttack = new MeleeAttack();

    public int MaxComboCount => _maxComboCount;
    public SFXOneShot HitSFX => _hitSFX;
    public SFXOneShot FinisherSFX => _finisherSFX;

    public MeleeAttack GroundAttack => _groundAttack;
    public MeleeAttack GroundFinisher => _groundFinisher;
    public MeleeAttack AirAttack => _airAttack;
    public MeleeAttack AirFinisher => _airFinisher;
    public MeleeAttack WallAttack => _wallAttack;
    public MeleeAttack WallFinisher => _wallFinisher;
    public MeleeAttack BounceAttack => _bounceAttack;
}
