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
    public event Action<IDamageable> HitDamageable;

    [SerializeField]
    private GameObject _forwardAttackCollision;
    [SerializeField]
    private GameObject _bounceAttackCollision;
    [SerializeField]
    private SpriteRenderer _weaponRenderer;
    [SerializeField]
    private WeaponData _equippedWeapon;
    [SerializeField]
    private WeaponAnimator _weaponAnimator;

    public WeaponData EquippedWeapon => _equippedWeapon;
    public MeleeAttack CurrentMeleeAttack { get; private set; }

    public MeleeAttackState MeleeAttackState { get; private set; } = MeleeAttackState.NotAttacking;

    public int AttackCount { get; private set; } = 0;

    private Coroutine _attackRoutine;

    private void Awake()
    {
        ActivateCollision(false);

        MeleeAttackState = MeleeAttackState.NotAttacking;
    }

    public void StartAttack(MeleeAttack meleeAttack, SFXOneShot hitSound, bool isInitialAttack)
    {
        if (meleeAttack == null) { return; }
        // if it's a combo, progress the counter, if not, start over
        if (isInitialAttack)
            AttackCount = 1;
        else
            AttackCount++;

        CurrentMeleeAttack = meleeAttack;

        if (_attackRoutine != null)
            StopCoroutine(_attackRoutine);
        _attackRoutine = StartCoroutine(AttackRoutine(meleeAttack.StartDelay, meleeAttack.ActiveDuration,
            meleeAttack.EndDelay, hitSound));
    }

    private void LoadAttackVisual(MeleeAttack meleeAttack)
    {
        if(AttackCount == EquippedWeapon.MaxComboCount)
        {
            _weaponAnimator.Play(WeaponAnimator.GroundSwingFinisherName);
        }
        // otherwise, alternate hit sprites
        else if(AttackCount % 2 == 0)
        {
            _weaponAnimator.Play(WeaponAnimator.GroundSwing02Name);
        }
        else
        {
            _weaponAnimator.Play(WeaponAnimator.GroundSwing01Name);
        }
    }

    public virtual void StopAttack()
    {
        if (_attackRoutine != null)
            StopCoroutine(_attackRoutine);
        ActivateCollision(false);
        _weaponAnimator.Stop();
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

    public void ActivateCollision(bool isActive)
    {
        _forwardAttackCollision.SetActive(isActive);
    }

    public void Hit(IDamageable damageable)
    {
        HitDamageable?.Invoke(damageable);
        damageable.Damage(CurrentMeleeAttack.Damage);
    }

    IEnumerator AttackRoutine(float beforeDelay, float activeDuration, float endDelay, SFXOneShot sfx)
    {
        MeleeAttackState = MeleeAttackState.BeforeAttack;
        yield return new WaitForSeconds(beforeDelay);

        MeleeAttackState = MeleeAttackState.DuringAttack;
        AttackActivated?.Invoke(EquippedWeapon);
        //TODO: check/deal damage here
        ActivateCollision(true);
        LoadAttackVisual(CurrentMeleeAttack);
        sfx.PlayOneShot(transform.position);
        yield return new WaitForSeconds(activeDuration);
        ActivateCollision(false);
        AttackDeactivated?.Invoke();

        MeleeAttackState = MeleeAttackState.AfterAttack;
        _weaponAnimator.Stop();
        //TODO: this could be a window for followup/combo attacks
        yield return new WaitForSeconds(endDelay);

        MeleeAttackState = MeleeAttackState.NotAttacking;
        AttackCompleted?.Invoke();
    }
}
