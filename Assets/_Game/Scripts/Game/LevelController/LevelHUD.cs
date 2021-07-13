using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelHUD : MonoBehaviour
{
    [SerializeField]
    private HUDScreen _introScreen;
    [SerializeField]
    private HUDScreen _winScreen;
    [SerializeField]
    private HUDScreen _loseScreen;

    public HUDScreen IntroScreen => _introScreen;
    public HUDScreen WinScreen => _winScreen;
    public HUDScreen LoseScreen => _loseScreen;

    public void DisableAllCanvases()
    {
        _introScreen.Hide();
        _winScreen.Hide();
        _loseScreen.Hide();
    }
}
