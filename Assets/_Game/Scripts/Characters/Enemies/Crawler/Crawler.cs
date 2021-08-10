using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crawler : Enemy
{
    [Header("Crawler Data")]
    [SerializeField]
    private float _movementSpeed = 3f;
    [SerializeField]
    private bool _reverseAtLedge = true;

    public float MovementSpeed => _movementSpeed;
    public bool ReverseAtLedge => _reverseAtLedge;
}
