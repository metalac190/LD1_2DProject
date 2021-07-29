using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// This component is used to push .objects with hit knockback.
/// It has been built to work with both custom physics as well as RB physics,
/// if needed.
/// </summary>
public class ReceiveKnockback : MonoBehaviour, IPushable
{
    [SerializeField]
    private Rigidbody2D _rb;
    [Range(0,1)][Tooltip("0 = no knockback, 1 = full knockback")]
    [SerializeField] float _knockbackDampener = 1;

    // this is used to try and make dynamic forces roughly match KM forces
    const float _dynamicForceMultiplier = 75;
    const float _dynamicDragMultiplier = 6;

    public event Action KnockbackStarted;
    public event Action KnockbackEnded;

    private MovementKM _kinematicObject;
    private Coroutine _knockbackRoutine;

    private void Awake()
    {
        _kinematicObject = GetComponent<MovementKM>();

        _rb.drag = _dynamicDragMultiplier;
    }

    public void Push(Vector2 direction, float knockbackAmount, float knockbackDuration)
    {
        // dampener scales value from 0 to full, using 0-1 input.
        // This allows this object to specify resistance to knockback if desired
        float dampenedDuration = knockbackDuration * _knockbackDampener;
        float dampenedAmount = knockbackAmount * _knockbackDampener;

        if (_kinematicObject != null)
            _kinematicObject.Push(direction, dampenedAmount, dampenedDuration);
        else
            PushRB(direction, dampenedAmount, dampenedDuration);

        // start our 'knockback call' for other things to watch, if needed
        if (_knockbackRoutine != null)
            StopCoroutine(_knockbackRoutine);
        _knockbackRoutine = StartCoroutine(KnockbackRoutine(dampenedDuration));
    }

    private void PushRB(Vector2 direction, float knockbackAmount, float knockbackDuration)
    {
        // we need to apply extra force to make dyanmic RB roughly behave similar to our custom physics
        knockbackAmount *= _dynamicForceMultiplier;
        _rb.AddForce(direction * knockbackAmount);
    }

    private IEnumerator KnockbackRoutine(float duration)
    {
        KnockbackStarted?.Invoke();
        yield return new WaitForSeconds(duration);
        KnockbackEnded?.Invoke();
    }

}
