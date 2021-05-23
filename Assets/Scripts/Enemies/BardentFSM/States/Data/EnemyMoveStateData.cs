using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data_MoveState_", menuName = "Data/StateData/MoveState")]
public class EnemyMoveStateData : ScriptableObject
{
    [SerializeField] private float _movementSpeed = 3;

    public float MovementSpeed => _movementSpeed;
}
