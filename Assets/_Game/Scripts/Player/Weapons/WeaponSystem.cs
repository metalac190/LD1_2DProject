using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WeaponSystem : MonoBehaviour
{
    public event Action<WeaponData> AttackActivated;
    public event Action AttackCompleted;

    [SerializeField]
    private GameObject _weaponVisual;
    [SerializeField]
    private WeaponData _equippedWeapon;

    public WeaponData EquippedWeapon => _equippedWeapon;
    // this specifically returns true while attack is active, and false during wind up and wind down periods
    public bool IsPreAttack { get; private set; } = false;
    public bool IsAttackActive { get; private set; } = false;
    public bool IsPostAttack { get; private set; } = false;

    private Coroutine _attackRoutine;

    private void Awake()
    {
        ShowVisual(false);
    }

    public virtual void StartAttack()
    {
        Debug.Log("Attack Start");

        ResetAttackBools();

        if (_attackRoutine != null)
            StopCoroutine(_attackRoutine);
        _attackRoutine = StartCoroutine(AttackRoutine(_equippedWeapon.StartDelay, 
            _equippedWeapon.ActiveDuration, _equippedWeapon.EndDelay));
    }

    public virtual void StopAttack()
    {
        Debug.Log("Attack Stop");

        if (_attackRoutine != null)
            StopCoroutine(_attackRoutine);
        ShowVisual(false);
        ResetAttackBools();
    }

    private void ResetAttackBools()
    {
        IsPreAttack = false;
        IsAttackActive = false;
        IsPostAttack = false;
    }

    public void EquipWeapon(WeaponData newWeapon)
    {
        if(newWeapon != null)
        {
            _equippedWeapon = newWeapon;
        }
    }

    public void ShowVisual(bool isActive)
    {
        _weaponVisual.SetActive(isActive);
    }

    IEnumerator AttackRoutine(float beforeDelay, float activeDuration, float endDelay)
    {
        ResetAttackBools();

        IsPreAttack = true;
        yield return new WaitForSeconds(beforeDelay);
        IsPreAttack = false;

        IsAttackActive = true;
        //TODO: check/deal damage here
        ShowVisual(true);
        _equippedWeapon.AttackSFX?.PlayOneShot(transform.position);
        yield return new WaitForSeconds(activeDuration);
        IsAttackActive = false;

        IsPostAttack = true;
        ShowVisual(false);
        yield return new WaitForSeconds(endDelay);
        IsPostAttack = false;

        AttackCompleted?.Invoke();
    }
}
