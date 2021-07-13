using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WinTrigger : MonoBehaviour
{
    public event Action PlayerEntered;

    private void OnTriggerEnter2D(Collider2D otherCollider)
    {
        Player player = otherCollider.gameObject.GetComponent<Player>();
        if(player != null)
        {
            Debug.Log("Trigger entered");
            PlayerEntered?.Invoke();
        }
    }
}
