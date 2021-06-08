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

    public int MaxComboCount => _maxComboCount;
    public SFXOneShot HitSFX => _hitSFX;
    public SFXOneShot FinisherSFX => _finisherSFX;

    public MeleeAttack GroundAttack => _groundAttack;
    public MeleeAttack GroundFinisher => _groundFinisher;
    public MeleeAttack AirAttack => _airAttack;
    public MeleeAttack AirFinisher => _airFinisher;
    public MeleeAttack WallAttack => _wallAttack;
    public MeleeAttack WallFinisher => _wallFinisher;



    /*
    [Header("Ground Settings")]
    [SerializeField]
    private int _groundHitDamage = 5;
    [SerializeField]
    private float _groundHitStartDelay = 0;
    [SerializeField]
    private float _groundHitActiveDuration = .1f;
    [SerializeField]
    private float _groundHitEndDelay = .1f;
    [SerializeField]
    private float _groundHitForwardAmount = 5;
    [SerializeField]
    private SFXOneShot _groundHitSFX;


    [Header("Ground Finisher")]
    [SerializeField]
    private int _groundFinisherDamage = 10;
    [SerializeField]
    private float _groundFinisherStartDelay = 0;
    [SerializeField]
    private float _groundFinisherActiveDuration = .15f;
    [SerializeField]
    private float _groundFinisherAfterDelay = .15f;
    [SerializeField]
    private SFXOneShot _groundFinisherSFX;

    [Header("Air Settings")]
    [SerializeField]
    private int _airDamageAmount = 5;
    [SerializeField]
    private float _airStartDelay = 0;
    [SerializeField]
    private float _airActiveDuration = .1f;
    [SerializeField]
    private float _airEndDelay = .1f;
    [SerializeField]
    private float _airForwardAmount = 0;
    [SerializeField]
    private SFXOneShot _airAttackSFX;
    [SerializeField]
    private float _airFinisherDelay = .15f;
    [SerializeField]
    private SFXOneShot _airFinisherSFX;

    ///
    [Header("Air Finisher")]
    [SerializeField]
    private int _airFinisherDamage = 10;

    public int MaxComboCount => _maxComboCount;
    public float MovementReductionRatio => _movementReductionRatio;
    // ground
    public int GroundHitDamage => _groundHitDamage;
    public float GroundHitStartDelay => _groundHitStartDelay;
    public float GroundHitActiveDuration => _groundHitActiveDuration;
    public float GroundHitEndDelay => _groundHitEndDelay;
    public float GroundHitForwardAmount => _groundHitForwardAmount;
    public SFXOneShot GroundHitSFX => _groundHitSFX;

    // ground finisher
    public int GroundFinisherDamage => _groundFinisherDamage;
    public float GroundFinisherStartDelay => _groundFinisherStartDelay;
    public float GroundFinisherActiveDuration => _groundFinisherActiveDuration;
    public float GroundFinisherAfterDelay => _groundFinisherAfterDelay;
    public float GroundFinisherForwardAmount => _groundHitForwardAmount;
    public SFXOneShot GroundFinisherSFX => _groundFinisherSFX;
    // air
    public int AirDamageAmount => _airDamageAmount;

    public float AirStartDelay => _airStartDelay;
    public float AirActiveDuration => _airActiveDuration;
    public float AirEndDelay => _airEndDelay;
    public float AirForwardAmount => _airForwardAmount;
    public SFXOneShot AirAttackSFX => _airAttackSFX;
    public float AirFinisherDelay => _airFinisherDelay;
    public SFXOneShot AirFinisherSFX => _airFinisherSFX;

    // air finisher
    public int AirFinisherDamage => _airFinisherDamage;
    */
}
