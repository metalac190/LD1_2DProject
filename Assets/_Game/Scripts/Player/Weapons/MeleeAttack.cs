using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SoundSystem;

[System.Serializable]
public class MeleeAttack
{
    [SerializeField]
    private int _damage = 5;
    [SerializeField]
    private float _startDelay = 0;
    [SerializeField]
    private float _activeDuration = .1f;
    [SerializeField]
    private float _endDelay = .1f;
    [SerializeField]
    private float _forwardAmount = 5;
    [SerializeField]
    [Range(0, 1)]
    [Tooltip("Scale from 0 - 1 on how much move speed is reduced during attack")]
    private float _movementReductionRatio = .2f;

    // hit01 and hit02 sprites will alternate until finisher sprite
    [SerializeField]
    private Sprite _spritePrimary;
    [SerializeField]
    private Sprite _spriteSecondary;

    public int Damage => _damage;
    public float StartDelay => _startDelay;
    public float ActiveDuration => _activeDuration;
    public float EndDelay => _endDelay;
    public float ForwardAmount => _forwardAmount;
    public float MovementReductionRatio => _movementReductionRatio;

    public Sprite SpritePrimary => _spritePrimary;
    public Sprite SpriteSecondary => _spriteSecondary;
}
