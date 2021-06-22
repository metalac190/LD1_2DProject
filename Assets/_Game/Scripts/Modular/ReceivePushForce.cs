using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// This component is used to push things that are NOT a Kinematic Object
/// </summary>
public class ReceivePushForce : MonoBehaviour, IPushable
{
    [SerializeField]
    private Rigidbody2D _rb;
    [Range(0,1)][Tooltip("0 = no knockback, 1 = full knockback")]
    [SerializeField] float _knockbackDampener = 1;

    // this is used to try and make dynamic forces roughly match KM forces
    const float _forceMultiplier = 50;

    public event Action KnockbackStarted;
    public event Action KnockbackEnded;

    private Coroutine _knockbackRoutine;

    public void Push(Vector2 direction, float knockbackAmount, float knockbackDuration)
    {
        // dampener scales value from 0 to full, using 0-1 input
        float dampenedAmount = knockbackAmount * _knockbackDampener * _forceMultiplier;
        float dampenedDuration = knockbackDuration * _knockbackDampener;
        _rb.AddForce(direction * dampenedAmount);

        Debug.Log("PUSHED: " + direction * dampenedAmount);
        if (_knockbackRoutine != null)
            StopCoroutine(_knockbackRoutine);
        _knockbackRoutine = StartCoroutine(KnockbackRoutine(dampenedDuration));
    }

    private IEnumerator KnockbackRoutine(float duration)
    {
        KnockbackStarted?.Invoke();
        yield return new WaitForSeconds(duration);
        KnockbackEnded?.Invoke();
    }

}
