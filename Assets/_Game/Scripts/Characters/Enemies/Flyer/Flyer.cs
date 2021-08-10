using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flyer : Enemy
{
    [Header("Flyer Data")]
    [SerializeField]
    private float _chaseSpeed = 5;
    [SerializeField]
    private float _returnSpeed = 8;

    public Vector3 StartPosition { get; private set; }

    public float ChaseSpeed => _chaseSpeed;
    public float ReturnSpeed => _returnSpeed;

    private void Awake()
    {
        StartPosition = transform.position;
    }
}
