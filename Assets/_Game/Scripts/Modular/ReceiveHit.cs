using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Receive hits from the player's weapon
/// </summary>
public class ReceiveHit : MonoBehaviour, IHitable
{
    public UnityEvent HitReceived;

    [SerializeField]
    private Health _health;
    [SerializeField]
    private ReceiveKnockback _receiveKnockback;

    public void Hit(HitData hitData)
    {
        Debug.Log("Hit");
        if(_health != null)
        {
            _health.Damage(hitData.Damage);
        }

        if(_receiveKnockback != null)
        {
            _receiveKnockback.Push(hitData.Direction, hitData.KnockbackForce, hitData.KnockbackDuration);
        }

        HitReceived?.Invoke();
    }
}
