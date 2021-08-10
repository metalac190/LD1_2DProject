using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour
{
    [Header("Actor Dependencies")]
    [SerializeField]
    private MovementKM _movement;
    [SerializeField]
    private EnvironmentDetector _environmentDetector;
    [SerializeField]
    private ReceiveHit _receiveHit;
    [SerializeField]
    private Health _health;


    public MovementKM Movement => _movement;
    public EnvironmentDetector EnvironmentDetector => _environmentDetector;
    public ReceiveHit ReceiveHit => _receiveHit;
    public Health Health => _health;
}
