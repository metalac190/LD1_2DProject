using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GenericTrigger : MonoBehaviour
{
    [SerializeField]
    private LayerMask _layersDetected;

    public UnityEvent OnTriggerEnter;
    public UnityEvent OnTriggerExit;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // if this collision is not in a detectable layer, return
        if (!PhysicsHelper.IsInLayerMask(collision.gameObject, _layersDetected))
            return;

        OnTriggerEnter?.Invoke();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // if this collision is not in a detectable layer, return
        if (!PhysicsHelper.IsInLayerMask(collision.gameObject, _layersDetected))
            return;

        OnTriggerExit?.Invoke();
    }
}
