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
    private Coroutine _cooldownRoutine;

    private bool _canDash = true;
    public bool CanDash => _canDash;

    public void StartCooldown(float duration)
    {
        Debug.Log("Start Dash Cooldown");
        if (_cooldownRoutine != null)
            StopCoroutine(_cooldownRoutine);
        _cooldownRoutine = StartCoroutine(CooldownRoutine(duration));
    }

    public void StopCooldown()
    {
        if (_cooldownRoutine != null)
            StopCoroutine(_cooldownRoutine);
        _canDash = true;
    }

    private IEnumerator CooldownRoutine(float duration)
    {
        _canDash = false;

        yield return new WaitForSeconds(duration);

        _canDash = true;
        Debug.Log("Cooldown Complete");
    }
}
