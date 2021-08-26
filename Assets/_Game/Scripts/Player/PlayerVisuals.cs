using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisuals : MonoBehaviour
{
    [SerializeField]
    private GameObject _ledgeHangVisual;
    [SerializeField]
    private ParticleSystem _jumpDust;
    [SerializeField]
    private ParticleSystem _deathParticlePrefab;

    public GameObject LedgeHangVisual => _ledgeHangVisual;
    public ParticleSystem JumpDust => _jumpDust;
    public ParticleSystem DeathParticlePrefab => _deathParticlePrefab;
}
