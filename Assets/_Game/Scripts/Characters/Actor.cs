using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour
{
    [Header("Actor Settings")]
    [SerializeField]
    private MovementKM _movement;
    [SerializeField]
    private EnvironmentDetector _environmentDetector;

    public MovementKM Movement => _movement;
    public EnvironmentDetector EnvironmentDetector => _environmentDetector;
}
