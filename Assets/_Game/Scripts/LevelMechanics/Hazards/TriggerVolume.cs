using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public abstract class TriggerVolume : MonoBehaviour
{
    public abstract void TriggerEntered(Collider2D collider);
    public abstract void TriggerExited(Collider2D collider);

    [SerializeField]
    private LayerMask _layersDetected;

    private Collider2D _collider;

    private void Awake()
    {
        // ensure it's marked as trigger
        _collider = GetComponent<Collider2D>();
        _collider.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D otherCollider)
    {
        // if we're not in the layer, return
        if (!PhysicsHelper.IsInLayerMask(otherCollider.gameObject, _layersDetected)) { return; }

        TriggerEntered(otherCollider);
    }

    private void OnTriggerExit2D(Collider2D otherCollider)
    {
        // if we're not in the layer, return
        if (!PhysicsHelper.IsInLayerMask(otherCollider.gameObject, _layersDetected)) { return; }

        TriggerExited(otherCollider);
    }
}
