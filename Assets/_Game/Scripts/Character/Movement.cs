using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D _rb;

    public int FacingDirection { get; private set; } = 1;
    public Rigidbody2D RB => _rb;
    public Vector2 Velocity => _rb.velocity;
    public Vector2 Position => _rb.position;

    public void HoldPosition(Vector2 position)
    {
        SetVelocityZero();
        // this seems redundant, but physics needs to explicitly be told that RB has stopped
        // moving or some systems (like Cinemachine) don't follow properly
        _rb.MovePosition(position);
    }

    public void SetVelocityY(float yVelocity)
    {
        _rb.velocity = new Vector2(_rb.velocity.x, yVelocity);
    }

    public void SetVelocityZero()
    {
        _rb.velocity = Vector2.zero;
    }

    public void SetVelocity(float velocity, Vector2 angle, int direction)
    {
        angle.Normalize();
        _rb.velocity = new Vector2(angle.x * velocity * direction, angle.y * velocity);
        CheckIfShouldFlip();
        //CheckIfShouldFlip(direction);
    }

    public void SetVelocity(float xVelocity, float yVelocity)
    {
        //CheckIfShouldFlip(_gameplayInput.XInputRaw);

        _rb.velocity = new Vector2(xVelocity, yVelocity);
        CheckIfShouldFlip();
    }

    public void SetVelocityX(float xVelocity)
    {
        //CheckIfShouldFlip(_gameplayInput.XInputRaw);
        _rb.velocity = new Vector2(xVelocity, _rb.velocity.y);
        CheckIfShouldFlip();
    }

    // moves to new position, accounting for path between points
    public void MovePositionSmooth(Vector2 newPosition)
    {
        _rb.MovePosition(newPosition);
    }

    public void MovePositionInstant(Vector2 newPosition)
    {
        _rb.position = newPosition;
    }

    public void SetDrag(float newDrag)
    {
        _rb.drag = newDrag;
    }

    public void Flip()
    {
        FacingDirection *= -1;
        _rb.transform.Rotate(0, 180, 0);
    }

    public void CheckIfShouldFlip()
    {
        int xDirection = Mathf.RoundToInt(Velocity.x);
        xDirection = Mathf.Clamp(xDirection, -1, 1);
        if (xDirection != 0 && xDirection != FacingDirection)
        {
            Flip();
        }
    }
}
