using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// For this solution we're not going to use Unity Physics, and instead attempt to
/// do all of our collision checks using periodic Overlap checks. This is mainly for optimization
/// and because our projectiles shoudln't need to interact with colliders much apart
/// from applying damage and occasionally being destroyed by things
/// </summary>
public abstract class ProjectileBase : MonoBehaviour
{
    public abstract void Move(float moveSpeed);

    [SerializeField]
    private float _moveSpeed = 5;
    [SerializeField]
    private float _detectRadius = 0.5f;
    [SerializeField]
    private LayerMask _layersToDamage;
    [SerializeField]
    private float _hitFrequency = .2f;

    [Header("Hit Info")]
    [SerializeField]
    private int _damage = 1;
    [SerializeField]
    private float _pushbackStrength = 20;
    [SerializeField]
    private float _pushbackDuration = .5f;

    private Coroutine _hitRoutine;

    private void OnEnable()
    {
        if(_hitRoutine != null)
            StopCoroutine(_hitRoutine);
        _hitRoutine = StartCoroutine(HitRoutine(_hitFrequency));
    }

    private void OnDisable()
    {
        if (_hitRoutine != null)
            StopCoroutine(_hitRoutine);
    }

    private void FixedUpdate()
    {
        Move(_moveSpeed);
    }

    private IEnumerator HitRoutine(float damageFrequency)
    {
        while (true)
        {
            HitOverlap();
            yield return new WaitForSeconds(damageFrequency);
        }
    }

    private void HitOverlap()
    {
        // detect nearby
        Collider2D colliderToHit = Physics2D.OverlapCircle(transform.position, 
            _detectRadius, _layersToDamage);
        // damage
        if(colliderToHit != null)
        {
            ApplyHit(colliderToHit);
        }
    }

    private void ApplyHit(Collider2D colliderToDamage)
    {
        ReceiveHit hitable = colliderToDamage.GetComponent<ReceiveHit>();
        if (hitable != null)
        {
            Vector2 direction = PhysicsHelper.ReverseVector
                (transform.position, hitable.transform.position);
            HitData hitData = new HitData
                (hitable.transform, _damage, 
                direction, _pushbackStrength, _pushbackDuration);
            hitable.Hit(hitData);
        }
    }
}
