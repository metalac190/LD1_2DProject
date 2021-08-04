using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This object tracks data relevant to the current running game sessions.
/// Using SIngleton for simplified game session data. Can turn this into a save/load system
/// on file later, if needed.
/// </summary>

public class GameSession : SingletonMBPersistent<GameSession>
{
    public Vector3 SpawnLocation { get; set; } = Vector3.zero;

    public int DeathCount { get; set; } = 0;
    public int FragmentCount { get; set; } = 0;
    public int ArtifactCount { get; set; } = 0;
    public int KeyCount { get; set; } = 0;

    public bool IsFirstAttempt => DeathCount <= 0;

    public void ClearGameSession()
    {
        SpawnLocation = Vector3.zero;
        DeathCount = 0;
        FragmentCount = 0;
        ArtifactCount = 0;
        KeyCount = 0;
    }

    public void SavePlayerData(Vector3 spawnPoint, Player player)
    {
        SpawnLocation = spawnPoint;

        FragmentCount = player.Inventory.Fragments;
        ArtifactCount = player.Inventory.Artifacts;
        KeyCount = player.Inventory.Keys;
    }

    public void LoadPlayerData(Player player)
    {
        player.Inventory.Fragments = FragmentCount;
        player.Inventory.Artifacts = ArtifactCount;
        player.Inventory.Keys = KeyCount;
    }
}
