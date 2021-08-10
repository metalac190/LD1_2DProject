using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetector : MonoBehaviour
{
    [SerializeField]
    private OverlapDetector _playerInRange;
    [SerializeField]
    private OverlapDetector _playerClose;
    [SerializeField]
    private RayDetector _playerLOS;

    public OverlapDetector PlayerInRange => _playerInRange;
    public OverlapDetector PlayerClose => _playerClose;
    public RayDetector PlayerLOS => _playerLOS;
}
