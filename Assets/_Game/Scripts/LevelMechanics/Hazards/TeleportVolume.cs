using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportVolume : TriggerVolume
{
    [Header("Teleport Settings")]
    [SerializeField]
    private Transform _exitTransform;
    [SerializeField]
    private ParticleSystem _exitParticles;

    public override void TriggerEntered(Collider2D otherCollider)
    {
        MovementKM movement = otherCollider.GetComponent<MovementKM>();
        if(movement != null)
        {
            if(_exitParticles != null)
            {
                ParticleSystem exitParticles = Instantiate
                    (_exitParticles, _exitTransform.position, Quaternion.identity);
                exitParticles.Play();
            }

            movement.MovePositionInstant(_exitTransform.position);
            
        }
    }
}
