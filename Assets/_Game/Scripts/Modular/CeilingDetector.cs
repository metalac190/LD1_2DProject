using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CeilingDetector : MonoBehaviour
{
    public event Action FoundCeiling;
    public event Action LostCeiling;

    [SerializeField]
    private Transform _ceilingCheckLocation;
    [SerializeField]
    private float _ceilingCheckRadius;
    [SerializeField]
    private LayerMask _whatIsCeiling;
    [SerializeField]
    private bool _autoCheck = false;

    public LayerMask WhatIsCeiling => _whatIsCeiling;

    private bool _isCeiling = false;
    public bool IsTouchingCeiling
    {
        get => _isCeiling;
        private set
        {
            // if our ceiling presence is about to change
            if (value != _isCeiling)
            {
                // if we're just now finding a ceiling
                if (value == true)
                {
                    FoundCeiling?.Invoke();
                }
                // or if we're just about to lose the ceiling we've already found
                else if (value == false)
                {
                    LostCeiling?.Invoke();
                }
            }

            _isCeiling = value;
        }
    }

    private void FixedUpdate()
    {
        if (_autoCheck)
            DetectCeiling();
    }

    public bool DetectCeiling()
    {
        if (_ceilingCheckLocation != null)
        {
            
            IsTouchingCeiling = Physics2D.OverlapCircle(_ceilingCheckLocation.position,
                _ceilingCheckRadius, _whatIsCeiling);
            return IsTouchingCeiling;
        }
        else
        {
            Debug.LogWarning("No groundcheck specified on: " + gameObject.name);
            return false;
        }

    }

    private void OnDrawGizmos()
    {
        if (_ceilingCheckLocation != null)
        {
            Gizmos.DrawWireSphere(_ceilingCheckLocation.position, _ceilingCheckRadius);
        }
    }
}
