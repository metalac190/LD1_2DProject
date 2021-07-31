using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// This script can be attached to a GameObject with a Trigger Volume to consistently apply damage
/// to the specified layers. You should also mark the object as 'Trigger' for optimization purposes. You can
/// also ignore any specified colliders if you don't want an enemy to damage itself, for example.
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class DamageZone : MonoBehaviour
{
    [SerializeField]
    private int _damageAmount = 5;
    [SerializeField]
    private float _knockbackAmount = 5;
    [SerializeField]
    private float _knockbackDuration = .2f;
    [SerializeField]
    private float _damageFrequency = .5f;
    [SerializeField]
    private LayerMask _layersToDamage;
    [SerializeField][Tooltip("Drag any colliders to ignore here, " +
        "for example, this object's Primary collider for movement")]
    private Collider2D[] _ignoreColliders;

    private Collider2D _damageCollider;

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

    Coroutine _damageRoutine;

    private void Awake()
    {
        _damageCollider = GetComponent<Collider2D>();
        _damageCollider.isTrigger = true;
        // ignore any specified colliders
        foreach(Collider2D collider in _ignoreColliders)
        {
            Physics2D.IgnoreCollision(collider, _damageCollider);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // if we're not in the layer, return
        if (!PhysicsHelper.IsInLayerMask(collision.gameObject, _layersToDamage)) { return; }

        ReceiveHit hitable = collision.GetComponent<ReceiveHit>();
        if(hitable != null)
        {
            Vector2 reverseVector = PhysicsHelper.ReverseVector(transform.position, collision.transform.position);
            HitData hitData = new HitData(hitable.transform, 
                _damageAmount, reverseVector, _knockbackAmount, _knockbackDuration);
            hitable.Hit(hitData);
        }
        // temporarily disable hit damage
        if (_damageRoutine != null)
            StopCoroutine(_damageRoutine);
        _damageRoutine = StartCoroutine(DamageRoutine(_damageFrequency));
    }

    IEnumerator DamageRoutine(float damageFrequency)
    {
        _damageCollider.enabled = false;
        yield return new WaitForSeconds(damageFrequency);
        _damageCollider.enabled = true;
    }
}
