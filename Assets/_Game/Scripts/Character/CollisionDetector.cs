using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetector : MonoBehaviour
{
    [SerializeField]
    private GroundDetector _groundDetector;
    [SerializeField]
    private WallDetector _wallDetector;
    [SerializeField]
    private LedgeDetector _ledgeDetector;
    [SerializeField]
    private CeilingDetector _ceilingDetector;

    public GroundDetector GroundDetector => _groundDetector;
    public WallDetector WallDetector => _wallDetector;
    public LedgeDetector LedgeDetector => _ledgeDetector;
    public CeilingDetector CeilingDetector => _ceilingDetector;
}
