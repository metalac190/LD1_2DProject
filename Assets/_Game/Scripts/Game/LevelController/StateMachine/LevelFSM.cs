using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LevelController))]
public class LevelFSM : StateMachineMB
{
    private LevelController _controller;

    public LevelIntroState IntroState;
    public LevelActiveState ActiveState;
    public LevelPauseState PauseState;
    public LevelWinState WinState;
    public LevelLoseState LoseState;

    private void Awake()
    {
        _controller = GetComponent<LevelController>();

        IntroState = new LevelIntroState(this, _controller);
        ActiveState = new LevelActiveState(this, _controller);
        PauseState = new LevelPauseState(this);
        WinState = new LevelWinState(this);
        LoseState = new LevelLoseState(this, _controller);
    }

    private void Start()
    {
        ChangeState(IntroState);
    }
}
