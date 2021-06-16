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
    private PlayerAfterImagePool _afterImagePool;

    private bool _isReady = true;

    public PlayerAfterImagePool AfterImagePool => _afterImagePool;
    public bool IsReady => _isReady;

    private void Awake()
    {
        ShowDashReadyVisual(true);
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
        _isReady = false;
        ShowDashReadyVisual(false);
    }

    private void ShowDashReadyVisual(bool isActive)
    {
        _dashCooldownVisual.SetActive(isActive);
    }
}
