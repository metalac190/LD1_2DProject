using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyPickup : Pickup
{
    protected override void OnPickup(Player player)
    {
        player.Inventory.Keys++;
    }
}
