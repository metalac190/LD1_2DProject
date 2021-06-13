using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtifactPickup : Pickup
{
    protected override void OnPickup(Player player)
    {
        player.Inventory.Artifacts++;
    }
}
