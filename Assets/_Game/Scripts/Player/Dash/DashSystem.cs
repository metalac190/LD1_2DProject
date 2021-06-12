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

    private bool _isReady = true;

    public PlayerAfterImagePool AfterImagePool => _afterImagePool;
    public bool CanDash => _isReady && _data.AllowDash;

    private void Awake()
    {
        ShowDashReadyVisual(true);
    }

    public void ReadyDash()
    {
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
