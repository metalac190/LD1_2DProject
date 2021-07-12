using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using SoundSystem;

public class WeaponSystem : MonoBehaviour
{
    public event Action<WeaponData> AttackActivated;
    public event Action AttackDeactivated;
    public event Action AttackCompleted;
    public event Action HitOther;

    [SerializeField]
    private GameObject _standardAttackCollision;
    [SerializeField]
    private GameObject _bounceAttackCollision;
    [SerializeField]
    private GameObject _wallAttackCollision;
    [SerializeField]
    private WeaponData _equippedWeapon;
    [SerializeField]
    private PlayerAnimator _playerAnimator;

    public WeaponData EquippedWeapon => _equippedWeapon;
    public MeleeAttack CurrentMeleeAttack { get; private set; }
    public GameObject CurrentWeaponCollision { get; private set; }

    public MeleeAttackState MeleeAttackState { get; private set; } = MeleeAttackState.NotAttacking;

    public int AttackCount { get; private set; } = 0;

    private Coroutine _attackRoutine;

    private void Awake()
    {
        DisableAllWeaponCollisions();
        MeleeAttackState = MeleeAttackState.NotAttacking;
    }

    public void StandardAttack(MeleeAttack meleeAttack, SFXOneShot hitSound, bool isInitialAttack)
    {
        if (meleeAttack == null) { return; }
        // if it's a combo, progress the counter, if not, start over
        if (isInitialAttack)
            AttackCount = 1;
        else
            AttackCount++;

        CurrentMeleeAttack = meleeAttack;
        CurrentWeaponCollision = _standardAttackCollision;

        string animationName;
        if (AttackCount == EquippedWeapon.MaxComboCount)
        {
            animationName = PlayerAnimator.GroundFinisherName;
        }
        // otherwise, alternate hit sprites
        else if (AttackCount % 2 == 0)
        {
            animationName = PlayerAnimator.GroundAttack02Name;
        }
        else
        {
            animationName = PlayerAnimator.GroundAttack01Name;
        }
        // do the gameplay sequence
        if (_attackRoutine != null)
            StopCoroutine(_attackRoutine);
        _attackRoutine = StartCoroutine(AttackRoutine(meleeAttack.StartDelay, meleeAttack.ActiveDuration,
            meleeAttack.EndDelay, hitSound, _standardAttackCollision, animationName));
    }

    public void BounceAttack(MeleeAttack meleeAttack, SFXOneShot hitSound)
    {
        if(meleeAttack == null) { return; }

        CurrentMeleeAttack = meleeAttack;
        CurrentWeaponCollision = _bounceAttackCollision;
        
        if (_attackRoutine != null)
            StopCoroutine(_attackRoutine);
        _attackRoutine = StartCoroutine(AttackRoutine(meleeAttack.StartDelay, meleeAttack.ActiveDuration,
            meleeAttack.EndDelay, hitSound, _bounceAttackCollision, PlayerAnimator.BounceAttackName));
    }

    private void DisableAllWeaponCollisions()
    {
        _standardAttackCollision.SetActive(false);
        _bounceAttackCollision.SetActive(false);
        _wallAttackCollision.SetActive(false);
    }

    public virtual void StopAttack()
    {
        if (_attackRoutine != null)
            StopCoroutine(_attackRoutine);

        CurrentWeaponCollision.SetActive(false);
        //_weaponAnimator.Stop();
        MeleeAttackState = MeleeAttackState.NotAttacking;
    }

    public void EquipWeapon(WeaponData newWeapon)
    {
        if(newWeapon != null)
        {
            _equippedWeapon = newWeapon;
            //TODO: Resize weapon
        }
    }

    public void HitOtherObject()
    {
        HitOther?.Invoke();
    }

    IEnumerator AttackRoutine(float beforeDelay, float activeDuration, float endDelay, 
        SFXOneShot sfx, GameObject collisionObject, string animationName)
    {
        // start animation before hit, in case there's windup
        _playerAnimator.PlayAnimation(animationName);
        MeleeAttackState = MeleeAttackState.BeforeAttack;
        yield return new WaitForSeconds(beforeDelay);

        MeleeAttackState = MeleeAttackState.DuringAttack;
        AttackActivated?.Invoke(EquippedWeapon);
        //TODO: check/deal damage here
        CurrentWeaponCollision.SetActive(true);
        //_weaponAnimator.Play(animationName);
        sfx.PlayOneShot(transform.position);
        yield return new WaitForSeconds(activeDuration);
        CurrentWeaponCollision.SetActive(false);
        AttackDeactivated?.Invoke();

        MeleeAttackState = MeleeAttackState.AfterAttack;
        //_weaponAnimator.Stop();
        //TODO: this could be a window for followup/combo attacks
        yield return new WaitForSeconds(endDelay);

        MeleeAttackState = MeleeAttackState.NotAttacking;
        AttackCompleted?.Invoke();
    }
}
