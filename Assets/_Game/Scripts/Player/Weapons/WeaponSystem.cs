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

    [SerializeField]
    private GameObject _weaponCollision;
    [SerializeField]
    private WeaponData _equippedWeapon;

    public WeaponData EquippedWeapon => _equippedWeapon;
    // this specifically returns true while attack is active, and false during wind up and wind down periods

    public MeleeAttackState MeleeAttackState { get; private set; } = MeleeAttackState.NotAttacking;

    private Coroutine _attackRoutine;

    private void Awake()
    {
        ActivateCollision(false);

        MeleeAttackState = MeleeAttackState.NotAttacking;
    }

    public virtual void StartGroundAttack()
    {
        if (_attackRoutine != null)
            StopCoroutine(_attackRoutine);
        _attackRoutine = StartCoroutine(AttackRoutine(_equippedWeapon.GroundStartDelay, 
            _equippedWeapon.GroundActiveDuration, _equippedWeapon.GroundEndDelay, 
            _equippedWeapon.GroundAttackSFX));
    }

    public virtual void StartAirAttack()
    {
        if (_attackRoutine != null)
            StopCoroutine(_attackRoutine);
        _attackRoutine = StartCoroutine(AttackRoutine(_equippedWeapon.AirStartDelay,
            _equippedWeapon.AirActiveDuration, _equippedWeapon.AirEndDelay, 
            _equippedWeapon.AirAttackSFX));
    }

    public virtual void StopAttack()
    {
        if (_attackRoutine != null)
            StopCoroutine(_attackRoutine);
        ActivateCollision(false);
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
        _weaponCollision.SetActive(isActive);
    }

    IEnumerator AttackRoutine(float beforeDelay, float activeDuration, float endDelay, SFXOneShot sfx)
    {
        MeleeAttackState = MeleeAttackState.BeforeAttack;
        yield return new WaitForSeconds(beforeDelay);

        MeleeAttackState = MeleeAttackState.DuringAttack;
        AttackActivated?.Invoke(EquippedWeapon);
        //TODO: check/deal damage here
        ActivateCollision(true);
        sfx.PlayOneShot(transform.position);
        yield return new WaitForSeconds(activeDuration);
        ActivateCollision(false);
        AttackDeactivated?.Invoke();

        MeleeAttackState = MeleeAttackState.AfterAttack;
        //TODO: this could be a window for followup/combo attacks
        yield return new WaitForSeconds(endDelay);

        MeleeAttackState = MeleeAttackState.NotAttacking;
        AttackCompleted?.Invoke();
    }
}
