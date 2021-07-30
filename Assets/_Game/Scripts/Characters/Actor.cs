using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour
{
    [Header("Actor Settings")]
    [SerializeField]
    private MovementKM _movement;
    [SerializeField]
    private CollisionDetector _collisionDetector;

    public MovementKM Movement => _movement;
    public CollisionDetector CollisionDetector => _collisionDetector;
}
