using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Player Settings")]
    [SerializeField] 
    private InputManager _input;
    [SerializeField]
    private PlayerData _data;
    [SerializeField]
    private PlayerAnimator _playerAnimator;
    [SerializeField]
    private Rigidbody2D _rb;
    [SerializeField]
    private GroundDetector _groundDetector;
    [SerializeField]
    private WallDetector _wallDetector;

    public InputManager Input => _input;
    public PlayerData Data => _data;
    public PlayerAnimator PlayerAnimator => _playerAnimator;
    public Rigidbody2D RB => _rb;
    public GroundDetector GroundDetector => _groundDetector;
    public WallDetector WallDetector => _wallDetector;

    public int FacingDirection { get; private set; } = 1;

    public int JumpsRemaining { get; private set; }

    private void Awake()
    {
        ResetJumps();
    }

    public void SetVelocity(float velocity, Vector2 angle, int direction)
    {
        angle.Normalize();
        _rb.velocity = new Vector2(angle.x * velocity * direction, angle.y * velocity);
        CheckIfShouldFlip(direction);
    }

    public void SetVelocityX(float newXVelocity)
    {
        CheckIfShouldFlip((int)_input.XRaw);
        _rb.velocity = new Vector2(newXVelocity, _rb.velocity.y);
    }

    public void DecreaseJumpsRemaining() => JumpsRemaining--;
    public void ResetJumps() => JumpsRemaining = _data.AmountOfJumps;

    public void SetVelocityY(float newYVelocity)
    {
        _rb.velocity = new Vector2(_rb.velocity.x, newYVelocity);
    }

    public void Flip()
    {
        FacingDirection *= -1;
        transform.Rotate(0, 180, 0);
    }


    private void CheckIfShouldFlip(int xInput)
    {
        if (xInput != 0 && xInput != FacingDirection)
        {
            Flip();
        }
    }
}
