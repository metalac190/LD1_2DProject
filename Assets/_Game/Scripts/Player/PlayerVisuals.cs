using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisuals : MonoBehaviour
{
    [SerializeField]
    private GameObject _ledgeHangVisual;

    public GameObject LedgeHangVisual => _ledgeHangVisual;
}
