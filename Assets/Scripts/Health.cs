using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]
public class Health : MonoBehaviour
{
    public UnityEvent<int> Damaged;
    public UnityEvent Died;

    public event Action KnockbackStarted;
    public event Action KnockbackEnded;

    [Header("Health")]
    [SerializeField]
    private int _maxHealth = 50;
    [SerializeField]
    private bool _isDamageable = true;

    [Header("Knockback")]
    [SerializeField]
    private bool _applyKnockback = true;
    [SerializeField]
    private float _knockbackSpeed = 7.5f;
    [SerializeField]
    private float _knockbackDuration = .1f;

    private float _currentHealth;

    private bool _isBeingKnockedBack = false;

    private Coroutine _knockbackRoutine;
    private Rigidbody2D _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();

        _currentHealth = _maxHealth;
    }

    public void TakeDamage(Transform sourceTransform, int amount)
    {
        if (!_isDamageable) return;

        Debug.Log("Damaged");
        _currentHealth -= amount;
        //TODO Hit Particles

        if(_applyKnockback)
            ApplyKnockBack(sourceTransform);

        _currentHealth = Mathf.Clamp(_currentHealth, 0, _maxHealth);
        if(_currentHealth == 0)
        {
            Kill();
        }
    }

    public void Kill()
    {
        Debug.Log("Killed " + gameObject.name);
        gameObject.SetActive(false);
    }

    public void ApplyKnockBack(Transform sourceTransform)
    {
        _isBeingKnockedBack = true;
        // calculate reverse direction
        Vector2 pushDirection = (sourceTransform.position - transform.position) * -1;
        _rb.velocity = new Vector2(_knockbackSpeed * pushDirection.x, 
            _knockbackSpeed * pushDirection.y);

        if (_knockbackRoutine != null)
            StopCoroutine(_knockbackRoutine);
        _knockbackRoutine = StartCoroutine(KnockbackRoutine(_knockbackDuration));
    }

    IEnumerator KnockbackRoutine(float duration)
    {
        KnockbackStarted?.Invoke();

        yield return new WaitForSeconds(duration);

        KnockbackEnded?.Invoke();
        _isBeingKnockedBack = false;
        _rb.velocity = new Vector2(0, _rb.velocity.y);
    }
}
