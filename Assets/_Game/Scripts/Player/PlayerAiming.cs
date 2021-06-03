using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAiming : MonoBehaviour
{
    [SerializeField]
    private Player _player;
    [SerializeField]
    private GameObject _aimIndicator;

    public Vector2 AimDirection { get; private set; }
    public bool IsAiming { get; private set; } = false;

    private GameplayInput _input;
    private Camera _camera;

    private void Awake()
    {
        _camera = Camera.main;
        _aimIndicator.gameObject.SetActive(false);
    }

    public void StartAiming()
    {
        _input = _player.Input;

        _aimIndicator.gameObject.SetActive(true);
        IsAiming = true;
    }

    public void StopAiming()
    {
        _aimIndicator.gameObject.SetActive(false);
        IsAiming = false;
        AimDirection = Vector2.zero;
    }

    private void Update()
    {
        if (IsAiming)
        {
            CalculateAimDirection();
            UpdateIndicatorRotation();
        }
    }

    // note this currently only calculates with mouse input. For Controller support, will need
    // to process Aim Direction differently
    private Vector2 CalculateAimDirection()
    {
        AimDirection = _camera.ScreenToWorldPoint(_input.MousePosition) - transform.position;
        AimDirection.Normalize();
        Debug.Log("AimDirection: " + AimDirection);
        return AimDirection;
    }

    private void UpdateIndicatorRotation()
    {
        float angle = Vector2.SignedAngle(Vector2.right, AimDirection);
        _aimIndicator.transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
