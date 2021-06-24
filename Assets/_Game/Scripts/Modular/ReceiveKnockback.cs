using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// This component is used to push things that are NOT a Kinematic Object
/// </summary>
public class ReceiveKnockback : MonoBehaviour, IPushable
{
    [SerializeField]
    private Rigidbody2D _rb;
    [Range(0,1)][Tooltip("0 = no knockback, 1 = full knockback")]
    [SerializeField] float _knockbackDampener = 1;

    // this is used to try and make dynamic forces roughly match KM forces
    const float _dynamicForceMultiplier = 75;
    const float _dynamicDragMultiplier = 10;

    public event Action KnockbackStarted;
    public event Action KnockbackEnded;

    private KinematicObject _kinematicObject;
    private Coroutine _knockbackRoutine;

    private void Awake()
    {
        _kinematicObject = GetComponent<KinematicObject>();

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
