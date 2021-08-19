using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using SoundSystem;
using System;

/// <summary>
/// Receive hits from the player's weapon
/// </summary>S
public class ReceiveHit : MonoBehaviour, IHitable
{
    public UnityEvent HitReceived;
    public event Action HitRecovered;  // this event is called after recovery from knocbkac

    [Header("Main")]
    [SerializeField]
    private bool _isImmune = false;
    [SerializeField]
    private float _hitRecoverTime = .5f;
    [SerializeField]
    private bool _receiveHitsWhileRecovering = true;
    [Range(0, 1)]
    [Tooltip("0 = no knockback, 1 = full knockback")]
    [SerializeField] float _knockbackDampener = 1;

    [Header("Hit Effects")]
    [SerializeField]
    private Color _flashColor = Color.red;
    [SerializeField]
    private SpriteRenderer _renderer;
    [SerializeField]
    private SFXOneShot _hitSFX;

    private MovementKM _movement;
    private Health _health;
    private HitFlash _hitFlash;
    private Coroutine _hitRecoverRoutine;

    public bool IsImmune
    {
        get => _isImmune;
        set => _isImmune = value;
    }
    public bool IsRecovering { get; private set; } = false;

    private void Awake()
    {
        _movement = GetComponent<MovementKM>(); // if we have movement, use it
        _health = GetComponent<Health>();   // if we have health, use it

        if (_renderer != null)
            _hitFlash = new HitFlash(this, _renderer, _flashColor, _hitRecoverTime);
        else
            Debug.LogError("No renderer assigned to Health component");

        IsRecovering = false;
    }

    void OnDisable()
    {
        IsRecovering = false;
        _hitFlash?.StopFlash();
    }

    public void Hit(HitData hitData)
    {
        if(_isImmune) { return; }
        // if we're recovering from previous hit, don't receive this one yet
        //if (IsRecovering && !_receiveHitsWhileRecovering) { return; }

        // apply damage if we have health
        if (_health != null)
        {
            _health.Damage(hitData.Damage);
        }
        // apply push if we can receive it
        if(_movement != null)
        {
            Push(hitData.Direction, hitData.KnockbackForce, hitData.KnockbackDuration);
        }

        HitReceived?.Invoke();

        if (_hitRecoverRoutine != null)
            StopCoroutine(_hitRecoverRoutine);
        _hitRecoverRoutine = StartCoroutine(HitRecoverRoutine(_hitRecoverTime));
    }

    private void PlayFX()
    {
        _hitSFX?.PlayOneShot(transform.position);
        _hitFlash?.Flash();
    }

    IEnumerator HitRecoverRoutine(float duration)
    {
        IsRecovering = true;
        if (_receiveHitsWhileRecovering == false)
        {
            IsImmune = true;
        }
        PlayFX();

        yield return new WaitForSeconds(duration);

        IsRecovering = false;
        if (_receiveHitsWhileRecovering == false)
        {
            IsImmune = false;
        }
        HitRecovered?.Invoke();
    }

    public void Push(Vector2 direction, float knockbackAmount, float knockbackDuration)
    {
        // dampener scales value from 0 to full, using 0-1 input.
        // This allows this object to specify resistance to knockback if desired
        float dampenedDuration = knockbackDuration * _knockbackDampener;
        float dampenedAmount = knockbackAmount * _knockbackDampener;

        if (_movement != null)
            _movement.Push(direction, dampenedAmount, dampenedDuration);
    }
}
