using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectiblePickup : Pickup
{
    [SerializeField]
    private int _collectibleIncreaseAmount = 1;

    protected override void OnPickup(Player collector)
    {
        Debug.Log("Small Collectible");
        collector.Inventory.Collectibles += _collectibleIncreaseAmount;
    }
}
