using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDScreen : MonoBehaviour
{
    [SerializeField]
    private Canvas _canvas;

    public virtual void Display()
    {
        Debug.Log("Display!");
        if(_canvas.gameObject.activeSelf == false)
            _canvas.gameObject.SetActive(true);
    }

    public virtual void Hide()
    {
        if (_canvas.gameObject.activeSelf == true)
            _canvas.gameObject.SetActive(false);
    }
}
