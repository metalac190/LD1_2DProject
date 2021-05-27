using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] 
    private InputManager _input;

    public InputManager Input => _input;

    private void OnEnable()
    {
        Input.SpacebarPressed += OnSpacebarPressed;
        Input.SpacebarReleased += OnSpacebarReleased;
        Input.EscapePressed += OnEscapePressed;
        Input.EscapeReleased += OnEscapeReleased;
        Input.EnterPressed += OnEnterPressed;
        Input.EnterReleased += OnEnterReleased;
        Input.MouseLeftPressed += OnLMBPressed;
        Input.MouseLeftReleased += OnLMBReleased;
        Input.MouseRightPressed += OnRMBPressed;
        Input.MouseRightReleased += OnRMBReleased;
    }

    private void OnDisable()
    {
        Input.SpacebarPressed -= OnSpacebarPressed;
        Input.SpacebarReleased -= OnSpacebarReleased;
        Input.EscapePressed -= OnEscapePressed;
        Input.EscapeReleased -= OnEscapeReleased;
        Input.EnterPressed -= OnEnterPressed;
        Input.EnterReleased -= OnEnterReleased;
        Input.MouseLeftPressed -= OnLMBPressed;
        Input.MouseLeftReleased -= OnLMBReleased;
        Input.MouseRightPressed -= OnRMBPressed;
        Input.MouseRightReleased -= OnRMBReleased;
    }

    private void OnEnterPressed()
    {
        Debug.Log("Enter");
    }
    
    void OnEnterReleased()
    {
        Debug.Log("Enter release");
    }

    private void OnEscapePressed()
    {
        Debug.Log("Escape");
    }

    void OnEscapeReleased()
    {
        Debug.Log("Escape released");
    }

    private void OnLMBPressed()
    {
        Debug.Log("Left Mouse");
    }

    private void OnLMBReleased()
    {
        Debug.Log("LMB released");
    }

    private void OnRMBPressed()
    {
        Debug.Log("Right Mouse");
    }

    private void OnRMBReleased()
    {
        Debug.Log("RMB released");
    }

    private void OnSpacebarPressed()
    {
        Debug.Log("Space");
    }

    private void OnSpacebarReleased()
    {
        Debug.Log("Space released");
    }
}
