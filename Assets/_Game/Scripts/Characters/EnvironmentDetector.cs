using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentDetector : MonoBehaviour
{
    [SerializeField]
    private OverlapDetector _groundDetector;
    [SerializeField]
    private OverlapDetector _groundInFront;
    [SerializeField]
    private OverlapDetector _wallDetector;
    [SerializeField]
    private OverlapDetector _aboveWallDetector;
    [SerializeField]
    private OverlapDetector _ceilingDetector;

    public OverlapDetector GroundDetector => _groundDetector;
    public OverlapDetector GroundInFrontDetector => _groundInFront;
    public OverlapDetector WallDetector => _wallDetector;
    public OverlapDetector AboveWallDetector => _aboveWallDetector;
    public OverlapDetector CeilingDetector => _ceilingDetector;
}
