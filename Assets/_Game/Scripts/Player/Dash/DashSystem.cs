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
    [SerializeField]
    private GameObject _dashCooldownVisual;
    [SerializeField] 
    private PlayerAfterImagePool _afterImagePool;

    private bool _isReady = true;

    public PlayerAfterImagePool AfterImagePool => _afterImagePool;
    public bool IsReady => _isReady;

    private Coroutine _dashCooldownRoutine;

    private void Awake()
    {
        ShowDashReadyVisual(true);
        ReadyDash();
    }

    public void ReadyDash()
    {
        // if it's already ready, don't 'activate' it, it's an unnecessary call
        if (_isReady) { return; }

        _isReady = true;
        ShowDashReadyVisual(true);
    }

    public void UseDash()
    {
        if (_dashCooldownRoutine != null)
            StopCoroutine(_dashCooldownRoutine);
        _dashCooldownRoutine = StartCoroutine(DashCooldownRoutine(_data.DashCooldown));
    }

    private void ShowDashReadyVisual(bool isActive)
    {
        _dashCooldownVisual.SetActive(isActive);
    }

    private IEnumerator DashCooldownRoutine(float duration)
    {
        _isReady = false;
        ShowDashReadyVisual(false);

        yield return new WaitForSeconds(duration);

        _isReady = true;
        ShowDashReadyVisual(true);
    }
}
