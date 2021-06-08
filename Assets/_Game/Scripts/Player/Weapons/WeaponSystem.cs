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
    public MeleeAttack CurrentMeleeAttack { get; private set; }

    public MeleeAttackState MeleeAttackState { get; private set; } = MeleeAttackState.NotAttacking;

    private Coroutine _attackRoutine;

    private void Awake()
    {
        ActivateCollision(false);

        MeleeAttackState = MeleeAttackState.NotAttacking;
    }

    public void StartAttack(MeleeAttack meleeAttack, SFXOneShot hitSound)
    {
        if(meleeAttack == null) { return; }
        CurrentMeleeAttack = meleeAttack;

        if (_attackRoutine != null)
            StopCoroutine(_attackRoutine);
        _attackRoutine = StartCoroutine(AttackRoutine(meleeAttack.StartDelay,meleeAttack.ActiveDuration, 
            meleeAttack.EndDelay, hitSound));
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
