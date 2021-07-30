using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockbackOtherOnTouch : MonoBehaviour
{
    [Header("General")]
    [SerializeField] 
    private float _knockbackAmount = 7.5f;
    [SerializeField] 
    private float _knockbackDuration = .1f;

    [Header("Colliders")]
    [SerializeField]
    private Collider2D _pushTrigger;
    [SerializeField]
    private LayerMask _layersToPush;
    [SerializeField]
    [Tooltip("Drag any colliders to ignore here, " +
    "for example, this object's Primary collider for movement")]
    private Collider2D[] _ignoreColliders;

    private void Awake()
    {
        // enforce trigger volume
        _pushTrigger = GetComponent<Collider2D>();
        _pushTrigger.isTrigger = true;
        // ignore specified colliders
        foreach(Collider2D collider in _ignoreColliders)
        {
            Physics2D.IgnoreCollision(collider, _pushTrigger);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!PhysicsHelper.IsInLayerMask(collision.gameObject, _layersToPush)) { return; }

        IPushable pushable = collision.GetComponent<IPushable>();
        if(pushable != null)
        {
            Vector2 direction = (transform.position - collision.gameObject.transform.position) * -1;
            pushable.Push(direction, _knockbackAmount, _knockbackDuration);
        }
    }
}
