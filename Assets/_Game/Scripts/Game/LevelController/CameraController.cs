using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;


public class CameraController : MonoBehaviour
{
    [SerializeField]
    private CinemachineVirtualCamera _playerCamera;

    public CinemachineVirtualCamera PlayerCamera => _playerCamera;
    
    private CinemachineBasicMultiChannelPerlin _playerCameraShakeSettings;

    private Coroutine _shakeRoutine;

    private void Awake()
    {
        _playerCameraShakeSettings 
            = _playerCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        _playerCameraShakeSettings.m_AmplitudeGain = 0;
        _playerCameraShakeSettings.m_FrequencyGain = 5;
    }

    public void StartCameraShake(float intensity, float duration)
    {
        Debug.Log("Camera Shake");
        _shakeRoutine = StartCoroutine(ShakeRoutine(intensity, duration));
    }

    public void StopCameraShake()
    {
        if (_shakeRoutine != null)
            StopCoroutine(_shakeRoutine);
        // set default values
        SetDefaultShakeValues();
    }

    private void SetDefaultShakeValues()
    {
        _playerCameraShakeSettings.m_AmplitudeGain = 0;

    }

    private IEnumerator ShakeRoutine(float intensity, float duration)
    {
        for (float elapsed = 0; elapsed <= duration; elapsed += Time.deltaTime)
        {
            _playerCameraShakeSettings.m_AmplitudeGain 
                = Mathf.Lerp(intensity, 0, elapsed / duration);
            yield return null;
        }

        SetDefaultShakeValues();
    }
}
