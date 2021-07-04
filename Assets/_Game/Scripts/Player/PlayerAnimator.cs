using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField]
    private Animator _animator;
    [SerializeField]
    private SpriteRenderer _spriteRenderer;
    [SerializeField]
    private GameObject _crouchVisual;

    public SpriteRenderer SpriteRenderer => _spriteRenderer;

    //public GameObject MainVisual => _mainVisual;
    //public GameObject CrouchVisual => _crouchVisual;

    public const string IdleName = "Idle";
    public const string RunName = "Run";
    public const string JumpName = "Jump";
    public const string FallName = "Fall";
    public const string LandName = "Land";
    public const string GroundAttack01Name = "GroundAttack01";
    public const string GroundAttack02Name = "GroundAttack02";
    public const string GroundFinisherName = "GroundFinisher";
    public const string BounceAttackName = "BounceAttack";
    public const string AirAttack01Name = "AirAttack01";
    public const string AirAttack02Name = "AirAttack02";
    public const string AirFinisher = "AirFinisher";
    public const string WallAttack = "WallAttack";
    public const string WallGrabName = "WallGrab";
    public const string WallSlideName = "WallSlide";

    public void PlayAnimation(string AnimationName)
    {
        _animator.Play(AnimationName);
    }


    public void ShowCrouchVisual(bool isActive)
    {
        /*
        if (isActive)
        {
            _crouchVisual.SetActive(true);
            _mainVisual.SetActive(false);
        }
        else
        {
            _crouchVisual.SetActive(false);
            _mainVisual.SetActive(true);
        }
        */
    }
}
