using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ObjectDetector : MonoBehaviour
{
    public event Action FoundObject;
    public event Action LostObject;

    [SerializeField]
    private LayerMask _layersToDetect;
    [SerializeField]
    private float _detectRadius = 25;

    [Header("Auto Detection")]
    // note: auto detection handles most things for you, at the expense of
    // knowing exactly when the query is happening
    [SerializeField]
    private bool _autoDetect = false;
    [SerializeField]
    private float _detectPlayerFrequency = .5f;

    public float DetectedDuration { get; private set; }
    public float DetectPlayerFrequency => _detectPlayerFrequency;
    public Collider2D[] DetectedObjects => _detectedObjects;
    public bool AutoDetect
    {
        get => _autoDetect;
        set => _autoDetect = value;
    }

    private Collider2D[] _detectedObjects;
    private Coroutine _detectRoutine;

    private bool _isObjectDetected = false;
    public bool IsObjectDetected
    {
        get => _isObjectDetected;
        private set
        {
            // if our detected state is about to change
            if (value != _isObjectDetected)
            {
                // if we're about to be detected, we have a new detected object
                if (value == true)
                {
                    DetectedDuration = 0;
                    FoundObject?.Invoke();
                }
                // if we're were previously detected and now we're not, we lost object
                else if (value == false)
                {
                    LostObject?.Invoke();
                }
            }
            _isObjectDetected = value;
        }
    }

    private void OnEnable()
    {
        if (_autoDetect)
        {
            _detectRoutine = StartCoroutine
                (DetectRoutine(_detectPlayerFrequency));
        }
    }

    private void OnDisable()
    {
        if (_autoDetect)
        {
            if (_detectRoutine != null)
                StopCoroutine(_detectRoutine);
        }
    }

    public bool DetectObject()
    {
        Debug.Log("Check for player");
        _detectedObjects = Physics2D.OverlapCircleAll(transform.position,
            _detectRadius, _layersToDetect);

        IsObjectDetected = _detectedObjects.Length > 0;
        Debug.Log("State: " + IsObjectDetected);

        return IsObjectDetected;
    }

    private IEnumerator DetectRoutine(float frequency)
    {
        while (true)
        {
            DetectObject();
            yield return new WaitForSeconds(frequency);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, _detectRadius);
    }
}
