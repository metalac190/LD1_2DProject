using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CrawlerData", menuName = "Data/Enemies/Crawler")]
public class CrawlerData : EnemyData
{
    [Header("Crawler")]
    [SerializeField]
    private float _movementSpeed = 3f;

    public float MovementSpeed => _movementSpeed;
}
