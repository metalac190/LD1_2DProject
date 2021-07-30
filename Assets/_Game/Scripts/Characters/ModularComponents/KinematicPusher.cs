using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KinematicPusher : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D _rb;

    private MovementKM _otherOverlappedMover;

    private void Awake()
    {
        _rb.isKinematic = true;
        _rb.useFullKinematicContacts = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        _otherOverlappedMover = collision.rigidbody.GetComponent<MovementKM>();

        if(_otherOverlappedMover != null)
        {
            _otherOverlappedMover.MoveOutOfCollider(collision.otherCollider);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(_otherOverlappedMover != null)
        {
            _otherOverlappedMover.MoveOutOfCollider(collision.otherCollider);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        _otherOverlappedMover = null;
    }

    /*
    private void RemoveOverlap(Collision2D collision)
    {
        // if we're supposed to be ignoring this layer, don't do anything
        if (_contactFilter.IsFilteringLayerMask(collision.collider.gameObject))
            return;
        // calculate collider distance
        ColliderDistance2D colliderDistance = Physics2D.Distance(collision.otherCollider, collision.collider);

        // if we're overlapped, remove it
        if (colliderDistance.isOverlapped)
        {
            collision.otherRigidbody.position += colliderDistance.normal 
                * (colliderDistance.distance);
        }
    }
    */
}
