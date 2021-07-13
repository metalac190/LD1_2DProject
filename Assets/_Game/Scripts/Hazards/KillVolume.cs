using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillVolume : MonoBehaviour
{
    [SerializeField]
    private LayerMask _layersToDamage;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // if we're not in the layer, return
        if (!PhysicsHelper.IsInLayerMask(collision.gameObject, _layersToDamage)) { return; }

        Health health = collision.GetComponent<Health>();
        if (health != null)
        {
            health.Kill();
        }
    }
}
