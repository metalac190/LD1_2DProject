using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data_IdleState_", menuName = "Data/StateData/IdleState")]
public class EnemyIdleStateData : ScriptableObject
{
    [SerializeField] float _minIdleTime = 1f;
    [SerializeField] float _maxIdleTime = 2f;

    public float MinIdleTime => _minIdleTime;
    public float MaxIdleTime => _maxIdleTime;
}
