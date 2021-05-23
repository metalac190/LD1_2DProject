using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data_Entity_", menuName = "Data/EntityData/BaseData")]
public class EntityData : ScriptableObject
{
    [SerializeField] private float _wallCheckDistance = 0.2f;
    [SerializeField] private float _ledgeCheckDistance = 0.4f;
    [SerializeField] private LayerMask _whatIsGround;

    public float WallCheckDistance => _wallCheckDistance;
    public float LedgeCheckDistance => _ledgeCheckDistance;
    public LayerMask WhatIsGround => _whatIsGround;
}
