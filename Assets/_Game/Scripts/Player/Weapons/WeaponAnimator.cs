using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAnimator : MonoBehaviour
{
    public const string Idle = "Idle";
    public const string GroundSwing01Name = "GroundSwing_01";
    public const string GroundSwing02Name = "GroundSwing_02";
    public const string GroundSwingFinisherName = "GroundFinisher";
    public const string BounceAttackName = "BounceAttack";

    [SerializeField]
    private Animator _animator;

    public void Play(string animationNodeName)
    {
        _animator.Play(animationNodeName);
    }

    public void Play(string animationNodeName, float blendTime)
    {
        _animator.CrossFadeInFixedTime(animationNodeName, blendTime);
    }

    public void Stop()
    {
        _animator.Play(Idle);
    }
}
