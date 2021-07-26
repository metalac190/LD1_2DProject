using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WinTrigger : TriggerVolume
{
    public event Action PlayerEntered;

    public override void TriggerEntered(Collider2D collider)
    {
        Player player = collider.gameObject.GetComponent<Player>();
        if (player != null)
        {
            Debug.Log("Trigger entered");
            PlayerEntered?.Invoke();
        }
    }
}
