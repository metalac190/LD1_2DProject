using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelHUD : MonoBehaviour
{
    [SerializeField]
    private IntroScreen _introScreen;
    [SerializeField]
    private WinScreen _winScreen;
    [SerializeField]
    private PlaytimeScreen _playtimeScreen;

    public IntroScreen IntroScreen => _introScreen;
    public WinScreen WinScreen => _winScreen;
    public PlaytimeScreen PlaytimeScreen => _playtimeScreen;

    public void DisableAllCanvases()
    {
        _introScreen.Hide();
        _winScreen.Hide();
        _playtimeScreen.Hide();
    }
}
