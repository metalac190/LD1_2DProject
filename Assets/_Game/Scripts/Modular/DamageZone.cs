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
    private float _damageFrequency = .5f;
    [SerializeField]
    private LayerMask _layersToDamage;
    [SerializeField][Tooltip("Drag any colliders to ignore here, " +
        "for example, this object's Primary collider for movement")]
    private Collider2D[] _ignoreColliders;

    private Collider2D _damageCollider;

    List<IDamageable> _damageableComponents = new List<IDamageable>();

    Coroutine _damageLoopRoutine;

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

    private void OnEnable()
    {
        if (_damageLoopRoutine != null)
            StopCoroutine(_damageLoopRoutine);
        _damageLoopRoutine = StartCoroutine(DamageLoopRoutine(_damageFrequency));
    }

    private void OnDisable()
    {
        if (_damageLoopRoutine != null)
            StopCoroutine(_damageLoopRoutine);

        _damageableComponents.Clear();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // if we're not in the layer, return
        if (!PhysicsHelper.IsInLayerMask(collision.gameObject, _layersToDamage)) { return; }

        IDamageable damageable = collision.GetComponent<IDamageable>();
        if(damageable != null)
        {
            Debug.Log("Adding damageable: " + damageable.ToString());
            DamageObject(damageable);
            // add it to our list to continue damaging independently, over time
            _damageableComponents.Add(damageable);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!PhysicsHelper.IsInLayerMask(collision.gameObject, _layersToDamage)) { return; }

        IDamageable damageable = collision.GetComponent<IDamageable>();
        if(damageable != null)
        {
            _damageableComponents.Remove(damageable);
        }
    }

    private IEnumerator DamageLoopRoutine(float frequency)
    {
        while (true)
        {
            yield return new WaitForSeconds(frequency);
            foreach (IDamageable damageable in _damageableComponents)
            {
                DamageObject(damageable);
            }
        }
    }

    public void DamageObject(IDamageable damageable)
    {
        damageable.Damage(_damageAmount);
    }
}
