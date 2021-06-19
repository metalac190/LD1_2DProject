using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour
{
    [SerializeField]
    private MovementKM _movement;
    [SerializeField]
    private CollisionDetector _collisionDetector;

    public MovementKM Movement => _movement;
    public CollisionDetector CollisionDetector => _collisionDetector;
}
