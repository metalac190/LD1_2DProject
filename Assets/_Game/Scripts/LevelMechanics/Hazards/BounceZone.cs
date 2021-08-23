using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SoundSystem;

public class BounceZone : TriggerVolume
{
    [Header("Bounce Settings")]
    [SerializeField]
    private float _bounceAmount = 20;
    [SerializeField]
    private float _bounceDuration = .5f;

    public override void TriggerEntered(Collider2D otherCollider)
    {
        // if it's the player, do player specific things
        MovementKM movement = otherCollider.gameObject.GetComponent<MovementKM>();
        if (movement != null)
        {
            movement.Push(transform.up, _bounceAmount, _bounceDuration);
        }
   }
}
