using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script will toggle the activate state of an objet on/off in a loop
/// indefinitely. Use this to create time-based level mechanics.
/// Put this script on the parent object, and make the gameObject you want to
/// toggle a child object. Make sure you fill in the object ref in this script
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class ObjectToggle : MonoBehaviour
{
    [Tooltip("Object that will turn on and off. Make this" +
        "a child object of this gameObject")]
    [SerializeField]
    private GameObject _toggleObject;
    [SerializeField]
    private float _startOffsetDuration = 0;

    [Header("On")]
    [SerializeField]
    private float _onDuration = 1;
    [SerializeField]
    private AudioClip _activeSound;

    [Header("Off")]
    [SerializeField]
    private float _offDuration = 1;

    private Coroutine _toggleRoutine;
    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        if(_activeSound != null)
            _audioSource.clip = _activeSound;
    }

    private void OnEnable()
    {
        StartToggle();
    }

    private void OnDisable()
    {
        StopToggle();
    }

    public void StartToggle()
    {
        _toggleRoutine = StartCoroutine(ToggleObject());
    }

    public void StopToggle()
    {
        if (_toggleRoutine != null)
            StopCoroutine(_toggleRoutine);
    }

    private IEnumerator ToggleObject()
    {
        yield return new WaitForSeconds(_startOffsetDuration);
        // infinite loop sequence, until disabled
        while (true)
        {
            // on
            _toggleObject.SetActive(true);
            if (_activeSound != null)
                _audioSource.Play();
            yield return new WaitForSeconds(_onDuration);
            if (_activeSound != null)
                _audioSource.Stop();
            // off
            _toggleObject.SetActive(false);
            yield return new WaitForSeconds(_offDuration);
        }
    }
}
