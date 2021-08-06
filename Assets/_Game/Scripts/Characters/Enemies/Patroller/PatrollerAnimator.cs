using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrollerAnimator : MonoBehaviour
{
    [SerializeField] Animator _animator;
    public Animator Animator => _animator;

    private const string IdleName = "PatrollerIdle";
    private const string MoveName = "PatrollerMove";
    private const string AttackName = "PatrollerAttack";
    private const string PlayerDetectedName = "PatrollerPlayerDetected";
    private const string ChargeName = "PatrollerCharge";
    private const string SearchName = "PatrollerSearch";
}
