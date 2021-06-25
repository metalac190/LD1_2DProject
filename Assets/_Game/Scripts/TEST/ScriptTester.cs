using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ScriptTester : MonoBehaviour
{
    [SerializeField]
    private MoveBetweenPoints _platform;

    void Update()
    {
        // on q press
        if (Keyboard.current.qKey.wasPressedThisFrame)
        {
            //_platform.Activate();
        }
        if (Keyboard.current.wKey.wasPressedThisFrame)
        {
            //_platform.Stop();
        }
    }
}
