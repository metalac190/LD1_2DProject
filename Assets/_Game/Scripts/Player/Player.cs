using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Player Settings")]
    [SerializeField]
    private GameplayInput _gameplayInput;
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
    [SerializeField]
    private LedgeDetector _ledgeDetector;

    public GameplayInput Input => _gameplayInput;
    public PlayerData Data => _data;
    public PlayerAnimator PlayerAnimator => _playerAnimator;
    public Rigidbody2D RB => _rb;
    public GroundDetector GroundDetector => _groundDetector;
    public WallDetector WallDetector => _wallDetector;
    public LedgeDetector LedgeDetector => _ledgeDetector;

    public int FacingDirection { get; private set; } = 1;

    public int AirJumpsRemaining { get; private set; }

    private void Awake()
    {
        ResetJumps();
    }

    public void HoldPosition(Vector2 position)
    {
        SetVelocityZero();
        // this seems redundant, but physics needs to explicitly be told that RB has stopped
        // moving or some systems (like Cinemachine) don't follow properly
        RB.MovePosition(position);
    }

    public void SetVelocityZero()
    {
        _rb.velocity = Vector2.zero;
    }

    public void SetVelocity(float velocity, Vector2 angle, int direction)
    {
        angle.Normalize();
        _rb.velocity = new Vector2(angle.x * velocity * direction, angle.y * velocity);
        CheckIfShouldFlip(direction);
    }

    public void SetVelocityX(float newXVelocity)
    {
        CheckIfShouldFlip(_gameplayInput.XRaw);
        _rb.velocity = new Vector2(newXVelocity, _rb.velocity.y);
    }

    public void DecreaseAirJumpsRemaining() => AirJumpsRemaining--;
    public void ResetJumps() => AirJumpsRemaining = _data.AmountOfAirJumps;

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
