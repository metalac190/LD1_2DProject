using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillVolume : TriggerVolume
{
    public override void TriggerEntered(Collider2D collider)
    {
        Health health = collider.GetComponent<Health>();
        if (health != null)
        {
            health.Kill();
        }
    }

    public override void TriggerExited(Collider2D collider)
    {
        //
    }
}
