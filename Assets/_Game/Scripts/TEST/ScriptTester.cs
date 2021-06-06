using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ScriptTester : MonoBehaviour
{
    [SerializeField]
    private Movement _movement;

    void Update()
    {
        // on q press
        if (Keyboard.current.qKey.wasPressedThisFrame)
        {
            //
        }
    }
}
