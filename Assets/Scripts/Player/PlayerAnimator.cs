using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField]
    private Animator _animator;

    public const string IdleName = "Idle";
    public const string WalkName = "Walk";

    public void PlayAnimation(string AnimationName)
    {
        //
    }
}
