using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportVolume : TriggerVolume
{
    [Header("Teleport Settings")]
    [SerializeField]
    private Transform _exitTransform;

    public override void TriggerEntered(Collider2D otherCollider)
    {
        MovementKM movement = otherCollider.GetComponent<MovementKM>();
        if(movement != null)
        {
            movement.MovePositionInstant(_exitTransform.position);
        }
    }
}
