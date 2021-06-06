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
    private GameObject _mainVisual;
    [SerializeField]
    private GameObject _crouchVisual;

    public SpriteRenderer SpriteRenderer => _spriteRenderer;

    public GameObject MainVisual => _mainVisual;
    public GameObject CrouchVisual => _crouchVisual;

    public const string IdleName = "Idle";
    public const string WalkName = "Walk";

    public void PlayAnimation(string AnimationName)
    {
        //
    }


    public void ShowCrouchVisual(bool isActive)
    {
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
    }
}
