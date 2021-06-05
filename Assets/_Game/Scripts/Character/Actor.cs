using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour
{
    [SerializeField]
    private Movement _movement;
    [SerializeField]
    private CollisionDetector _collisionDetector;

    public Movement Movement => _movement;
    public CollisionDetector CollisionDetector => _collisionDetector;
}
