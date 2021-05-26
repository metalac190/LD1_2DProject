using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ReceiveKnockback : MonoBehaviour
{
    [Range(0,1)][Tooltip("0 = no knockback, 1 = full knockback")]
    [SerializeField] float _knockbackDampener = 1;
    [SerializeField] float _upAmount = 8;

    public event Action KnockbackStarted;
    public event Action KnockbackEnded;

    private bool _isKnockbackHappening = false;

    public bool IsKnockbackHappening => _isKnockbackHappening;

    private Coroutine _knockbackRoutine;
    private Rigidbody2D _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    public void Knockback(float knockbackAmount, float knockbackDuration, Transform sourceTransform)
    {
        // dampener scales value from 0 to full, using 0-1 input
        float dampenedAmount = knockbackAmount * _knockbackDampener;
        float dampenedDuration = knockbackDuration * _knockbackDampener;
        // calculate reverse direction
        Vector2 pushDirection = ((sourceTransform.position - transform.position) * -1) * dampenedAmount;
        // combine push with upward force
        _rb.velocity = (pushDirection + (Vector2.up * _upAmount));

        if (_knockbackRoutine != null)
            StopCoroutine(_knockbackRoutine);
        _knockbackRoutine = StartCoroutine(KnockbackRoutine(dampenedDuration));
    }

    IEnumerator KnockbackRoutine(float duration)
    {
        _isKnockbackHappening = true;
        KnockbackStarted?.Invoke();

        yield return new WaitForSeconds(duration);

        _rb.velocity = new Vector2(0, _rb.velocity.y);
        _isKnockbackHappening = false;
        KnockbackEnded?.Invoke();
    }
}
