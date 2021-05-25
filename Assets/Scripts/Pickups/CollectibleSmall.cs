using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleSmall : Pickup
{
    protected override void OnPickup(GameObject collector)
    {
        Debug.Log("Small Collectible");
    }
}
