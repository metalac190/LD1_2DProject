using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleLarge : Pickup
{
    protected override void OnPickup(GameObject collector)
    {
        Debug.Log("Large Collectible");
    }
}
