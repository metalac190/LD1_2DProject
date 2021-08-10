using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patroller : Enemy
{
    [Header("Patroller Dependencies")]
    [SerializeField]
    private PatrollerData _data;
    [SerializeField]
    private PatrollerAnimator _patrollerAnimator;
    [SerializeField]
    private GameObject _detectedGraphic;

    public PatrollerData Data => _data;
    public PatrollerAnimator PatrollerAnimator => _patrollerAnimator;
    public GameObject DetectedGraphic => _detectedGraphic;

    private void Awake()
    {
        HitVolume.gameObject.SetActive(false);
        DetectedGraphic.gameObject.SetActive(false);
    }

    public void Remove()
    {
        Destroy(gameObject);
    }
}
