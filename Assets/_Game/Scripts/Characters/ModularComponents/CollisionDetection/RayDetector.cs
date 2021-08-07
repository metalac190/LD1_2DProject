using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
/// <summary>
/// This component casts rays in the forward direction to look for the player
/// </summary>
public class RayDetector : ColliderDetector
{
    [Header("Ray Settings")]
    [SerializeField]
    private float _detectDistance = 10;
    [SerializeField]
    private Vector2 _direction = new Vector2(1, 0);

    public override Collider2D Detect()
    {
        // cast in forward direction
        LastDetectedCollider = Physics2D.Raycast(transform.position, 
            _direction * transform.right,
            _detectDistance, DetectLayers).collider;

        IsDetected = LastDetectedCollider != null;

        return LastDetectedCollider;
    }

    protected virtual void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position
            + (transform.right * _detectDistance));
    }
}
