using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] 
    private InputManager _input;
    [SerializeField]
    private PlayerData _data;
    [SerializeField]
    private PlayerAnimator _playerAnimator;
    [SerializeField]
    private Rigidbody2D _rb;

    public InputManager Input => _input;
    public PlayerData Data => _data;
    public PlayerAnimator PlayerAnimator => _playerAnimator;
    public Rigidbody2D Rb => _rb;

    public int FacingDirection { get; private set; } = 1;
    
    public void SetVelocityX(float newXVelocity)
    {
        _rb.velocity = new Vector2(newXVelocity, _rb.velocity.y);
    }

    public void CheckIfShouldFlip(int xInput)
    {
        if(xInput != 0 && xInput != FacingDirection)
        {
            Flip();
        }
    }

    public void Flip()
    {
        FacingDirection *= -1;
        transform.Rotate(0, 180, 0);
    }
}
