using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverlapDetector : ColliderDetector
{
    [SerializeField]
    private float _detectRadius = 0.3f;

    public override Collider2D Detect()
    {
        LastDetectedCollider = Physics2D.OverlapCircle(transform.position,
            _detectRadius, DetectLayers);

        IsDetected = LastDetectedCollider != null;

        return LastDetectedCollider;
    }

    protected virtual void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _detectRadius);
    }
}
