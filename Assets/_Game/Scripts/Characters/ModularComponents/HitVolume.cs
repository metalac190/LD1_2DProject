using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// This script can be attached to a GameObject with a Trigger Volume to consistently apply damage
/// to the specified layers. You should also mark the object as 'Trigger' for optimization purposes. You can
/// also ignore any specified colliders if you don't want an enemy to damage itself, for example.
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class HitVolume : MonoBehaviour
{
    public Action<GameObject> Hit;

    [SerializeField]
    private int _damageAmount = 1;
    [SerializeField]
    private float _knockbackAmount = 5;
    [SerializeField]
    private Vector2 _knockbackDirection = new Vector2(1, 1); // note, direction relative to received being to the right
    [SerializeField]
    private float _knockbackDuration = .2f;
    [SerializeField]
    private float _hitFrequency = .5f;
    [SerializeField]
    private LayerMask _layersToHit;
    [SerializeField][Tooltip("Drag any colliders to ignore here, " +
        "for example, this object's Primary collider for movement")]
    private Collider2D[] _ignoreColliders;

    private Collider2D _hitCollider;

    public int DamageAmount
    {
        get => _damageAmount;
        set 
        {
            if(value < 0)
                value = 0;
            _damageAmount = value;
        } 
    }

    Coroutine _hitRoutine;

    private void Awake()
    {
        _hitCollider = GetComponent<Collider2D>();
        _hitCollider.isTrigger = true;
        // ignore any specified colliders
        foreach(Collider2D collider in _ignoreColliders)
        {
            Physics2D.IgnoreCollision(collider, _hitCollider);
        }

        _knockbackDirection.Normalize();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // if we're not in the layer, return
        if (!PhysicsHelper.IsInLayerMask(collision.gameObject, _layersToHit)) { return; }

        ReceiveHit hitable = collision.GetComponent<ReceiveHit>();
        if(hitable != null && hitable.IsImmune == false)
        {
            HitObject(collision, hitable);
        }
    }

    private void HitObject(Collider2D collision, ReceiveHit hitable)
    {
        float relativeDirection = PhysicsHelper.RelativeDirection
            (transform.position, collision.transform.position);
        Debug.Log("Relative Direction: " + relativeDirection);
        Vector2 knockbackDirection = new Vector2
            (_knockbackDirection.x * relativeDirection, _knockbackDirection.y);
        Debug.Log("Knockback Direction: " + knockbackDirection);
        //Vector2 reverseVector = PhysicsHelper.ReverseVector(transform.position, collision.transform.position);
        HitData hitData = new HitData(hitable.transform,
            _damageAmount, knockbackDirection, _knockbackAmount, _knockbackDuration);
        hitable.Hit(hitData);
        Hit?.Invoke(hitable.gameObject);
        // temporarily disable hit damage
        if (_hitRoutine != null)
            StopCoroutine(_hitRoutine);
        _hitRoutine = StartCoroutine(DamageRoutine(_hitFrequency));
    }

    IEnumerator DamageRoutine(float damageFrequency)
    {
        _hitCollider.enabled = false;
        yield return new WaitForSeconds(damageFrequency);
        _hitCollider.enabled = true;
    }
}
