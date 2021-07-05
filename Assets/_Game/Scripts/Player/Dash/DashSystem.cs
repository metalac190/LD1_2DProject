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
    public event Action DashCompleted;

    [SerializeField]
    private Player _player;
    [SerializeField]
    private GameObject _dashCooldownVisual;
    [SerializeField] 
    private PlayerAfterImagePool _afterImagePool;
    [SerializeField]
    private float _distanceBetweenAfterImages = 1.5f;

    private PlayerAfterImage _lastAfterImage;
    private bool _isReady = true;

    public PlayerAfterImagePool AfterImagePool => _afterImagePool;
    public bool IsReady => _isReady;

    private Coroutine _dashingRoutine;
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

    public void UseDash(float dashDuration, float dashCooldown)
    {
        if (_dashingRoutine != null)
            StopCoroutine(_dashingRoutine);
        _dashingRoutine = StartCoroutine(DashRoutine(dashDuration, dashCooldown));
    }

    public void StopDash(float cooldownDuration)
    {
        // kill our dash routine preemptively, if needed;
        if (_dashingRoutine != null)
            StopCoroutine(_dashingRoutine);

        // start cooldown
        if (_dashCooldownRoutine != null)
            StopCoroutine(_dashCooldownRoutine);
        _dashCooldownRoutine = StartCoroutine(DashCooldownRoutine(cooldownDuration));
    }

    private void ShowDashReadyVisual(bool isActive)
    {
        _dashCooldownVisual.SetActive(isActive);
    }

    private IEnumerator DashRoutine(float dashDuration, float cooldownDuration)
    {
        // ensure dash isn't reusable during dash
        _isReady = false;
        ShowDashReadyVisual(false);
        // perform the dash duration
        float dashElapsed = 0;
        while (dashElapsed < dashDuration)
        {
            _lastAfterImage = _afterImagePool.PlaceAfterImage(_player);

            yield return new WaitForFixedUpdate();
            CheckAfterImageSpawn();
            dashElapsed += Time.fixedDeltaTime;
        }
        // dash portion is completed
        DashCompleted?.Invoke();
    }

    private IEnumerator DashCooldownRoutine(float duration)
    {
        // ensure visual is still off, if cooldown triggered separate from dash
        _isReady = false;
        ShowDashReadyVisual(false);

        yield return new WaitForSeconds(duration);

        ShowDashReadyVisual(true);
        _isReady = true;
    }

    private void CheckAfterImageSpawn()
    {
        if (_lastAfterImage != null)
        {
            float imageDistance = Vector2.Distance(_player.transform.position, _lastAfterImage.transform.position);
            if (imageDistance >= _distanceBetweenAfterImages)
            {
                _lastAfterImage = _afterImagePool.PlaceAfterImage(_player);
            }
        }
    }
}
