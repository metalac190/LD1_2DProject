using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using SoundSystem;

/// <summary>
/// Receive hits from the player's weapon
/// </summary>
public class ReceiveHit : MonoBehaviour, IHitable
{
    public UnityEvent HitReceived;

    [SerializeField]
    private float _hitRecoverTime = .1f;

    [Header("Optional Components")]
    [SerializeField]
    private Health _health;
    [SerializeField]
    private ReceiveKnockback _receiveKnockback;

    [Header("Hit Effects")]
    [SerializeField]
    private Color _flashColor = Color.red;
    [SerializeField]
    private SpriteRenderer _renderer;
    [SerializeField]
    private SFXOneShot _hitSFX;

    private HitFlash _hitFlash;
    private Coroutine _hitRecoverRoutine;

    public bool IsRecovering { get; private set; } = false;

    private void Awake()
    {
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
        // if we're recovering from previous hit, don't receive this one yet
        if (IsRecovering) { return; }

        // apply damage if we have health
        if (_health != null)
        {
            _health.Damage(hitData.Damage);
        }
        // apply knockback if we can receive it
        if (_receiveKnockback != null)
        {
            _receiveKnockback.Push(hitData.Direction, hitData.KnockbackForce, hitData.KnockbackDuration);
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
        PlayFX();

        yield return new WaitForSeconds(duration);

        IsRecovering = false;
    }
}
