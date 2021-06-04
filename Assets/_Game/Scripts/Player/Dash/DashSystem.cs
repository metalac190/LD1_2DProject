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
    private GameObject _dashCooldownVisual;
    [SerializeField]
    private PlayerData _data;
    [SerializeField] 
    private PlayerAfterImagePool _afterImagePool;

    private Coroutine _cooldownRoutine;

    private bool _canDash = true;

    public PlayerAfterImagePool AfterImagePool => _afterImagePool;
    public bool CanDash => _canDash;

    private void Awake()
    {
        ShowDashReadyVisual(true);
    }

    public void StartCooldown(float duration)
    {
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
        ShowDashReadyVisual(false);

        yield return new WaitForSeconds(duration);

        _canDash = true;
        ShowDashReadyVisual(true);
    }


    public void ShowDashReadyVisual(bool isActive)
    {
        _dashCooldownVisual.SetActive(isActive);
    }
}
