using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// This class helps handle a lot of the persistent data and methods related to dashing.
/// The player state machine will drive a lot of the activation, this is mainly intended to
/// isolate dashing responsibility out of the player class and the dash state.
/// </summary>
public class DashSystem : MonoBehaviour
{
    [SerializeField]
    private PlayerData _data;

    public PlayerDashAbility EquippedDash { get; private set; }

    private float _dashCooldown = 0;
    private Coroutine _cooldownRoutine;

    private bool _canDash = true;
    public bool CanDash => (_canDash && EquippedDash != null);

    private void Awake()
    {
        Initialize(_data);
    }

    private void Initialize(PlayerData data)
    {
        EquipDash(data.EquippedDash);
    }

    public void StartCooldown()
    {
        Debug.Log("Start Dash Cooldown");
        if (_cooldownRoutine != null)
            StopCoroutine(_cooldownRoutine);
        _cooldownRoutine = StartCoroutine(CooldownRoutine(_dashCooldown));
    }

    public void StopCooldown()
    {
        if (_cooldownRoutine != null)
            StopCoroutine(_cooldownRoutine);
        _canDash = true;
    }

    public void EquipDash(PlayerDashAbility newDashAbility)
    {
        if(newDashAbility == null) { return; }

        EquippedDash = newDashAbility;
        _dashCooldown = newDashAbility.Cooldown;
    }

    private IEnumerator CooldownRoutine(float duration)
    {
        _canDash = false;
        yield return new WaitForSeconds(duration);
        _canDash = true;
    }
}
