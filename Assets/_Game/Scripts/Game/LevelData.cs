using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelData_", menuName = "Data/Game/Level Data")]
public class LevelData : ScriptableObject
{
    [SerializeField]
    private string _levelName = "Prototype";
    [SerializeField]
    private string _levelDescription = "...";

    public string LevelName => _levelName;
    public string LevelDescription => _levelDescription;
}
