using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HorizontalDash", menuName = "Data/Player/Dash Abilities/Horizontal Dash")]
public class HorizontalDash : PlayerDashAbility
{
    [Header("Horizontal Dash")]
    [SerializeField]
    private float _strength = 60;
    [SerializeField]
    private float _duration = .15f;

    private float _timer = 0;

    Coroutine _dashRoutine;

    public override event Action Completed;

    public override void OnInputPress(Player player)
    {
        base.OnInputPress(player);

        Timer.DelayActionRetriggerable(player, OnComplete, _duration, _dashRoutine);
    }

    public override void OnFixedUpdate(Player player)
    {
        player.SetVelocityX(_strength * player.FacingDirection);
        player.SetVelocityY(0);
    }

    private void OnComplete()
    {
        Completed?.Invoke();
    }
}
