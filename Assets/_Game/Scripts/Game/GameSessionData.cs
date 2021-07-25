using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This object tracks data relevant to the current running game sessions.
/// This is a simplified work-around to a save system, which might be overboard for the purposes
/// of this project and cause more confusion than it's worth. Long term this data could be saved
/// to file if needed.
/// </summary>

[CreateAssetMenu(fileName = "GameSessionData", menuName = "Data/Game/GameSession")]
public class GameSessionData : ScriptableObject
{
    public Vector3 SpawnLocation { get; set; } = Vector3.zero;
    public int DeathCount { get; set; } = 0;

    public bool IsFirstAttempt => DeathCount <= 0;

    public void ClearGameSession()
    {
        SpawnLocation = Vector3.zero;
        DeathCount = 0;
    }
}
