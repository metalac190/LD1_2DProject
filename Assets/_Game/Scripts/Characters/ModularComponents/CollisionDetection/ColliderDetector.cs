using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//TODO - make auto check active in Update NOT onenable ondisable
public abstract class ColliderDetector : MonoBehaviour
{
    public abstract Collider2D Detect();

    public event Action FoundCollider;
    public event Action LostCollider;

    [SerializeField]
    private LayerMask _detectLayers;

    [Header("Auto Detection")]
    // note: auto detection handles most things for you, at the expense of
    // knowing exactly when the query is happening
    [SerializeField]
    private bool _autoDetect = false;
    [SerializeField]
    private float _detectFrequency = .2f;

    private Collider2D _lastDetectedCollider;

    protected LayerMask DetectLayers => _detectLayers;

    public Collider2D LastDetectedCollider
    {
        get => _lastDetectedCollider;
        protected set
        {
            _lastDetectedCollider = value;
        } 
    }

    public float DetectFrequency => _detectFrequency;

    public float DetectedDuration { get; private set; }
    public float LostDetectionDuration { get; private set; }

    private Coroutine _detectRoutine;

    private bool _isDetected = false;
    public bool IsDetected
    {
        get => _isDetected;
        protected set
        {
            // if our detected state is about to change
            if (value != _isDetected)
            {
                // if we're about to be detected, we have a new detected object
                if (value == true)
                {
                    DetectedDuration = 0;
                    FoundCollider?.Invoke();
                }
                // if we're were previously detected and now we're not, we lost object
                else if (value == false)
                {
                    LostDetectionDuration = 0;
                    LostCollider?.Invoke();
                }
            }
            _isDetected = value;
        }
    }

    public void StartDetecting()
    {
        IsDetected = false;
        _detectRoutine = StartCoroutine
            (DetectRoutine(_detectFrequency));
    }

    public void StopDetecting()
    {
        if (_detectRoutine != null)
            StopCoroutine(_detectRoutine);
        IsDetected = false;
    }

    private void OnEnable()
    {
        if (_autoDetect)
        {
            StartDetecting();
        }
    }

    private void OnDisable()
    {
        StopDetecting();
    }

    private void Update()
    {
        if (IsDetected)
            DetectedDuration += Time.deltaTime;
        else
            LostDetectionDuration += Time.deltaTime;
    }

    private IEnumerator DetectRoutine(float frequency)
    {
        while (true)
        {
            _lastDetectedCollider = Detect();
            IsDetected = _lastDetectedCollider != null;

            yield return new WaitForSeconds(frequency);
        }
    }
}
