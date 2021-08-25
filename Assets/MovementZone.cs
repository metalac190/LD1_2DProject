using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementZone : TriggerVolume
{
    [Header("Movement Zone")]
    [SerializeField]
    private Vector2 _moveDirection = new Vector2(1, 0);
    [SerializeField]
    private float _moveSpeed = 3;

    private List<MovementKM> _passengers = new List<MovementKM>();

    protected override void Awake()
    {
        base.Awake();

        _moveDirection.Normalize();
    }

    private void FixedUpdate()
    {
        if (_passengers != null && _passengers.Count > 0)
        {
            foreach (MovementKM moveable in _passengers)
            {
                moveable.Move(_moveDirection * _moveSpeed, false);
            }
        }
    }

    public override void TriggerEntered(Collider2D otherCollider)
    {
        MovementKM movement = otherCollider.GetComponent<MovementKM>();
        if (movement != null)
        {
            _passengers.Add(movement);
        }
    }

    protected override void TriggerExited(Collider2D otherCollider)
    {
        base.TriggerExited(otherCollider);

        MovementKM movement = otherCollider.GetComponent<MovementKM>();
        if (movement != null)
        {
            _passengers.Remove(movement);
        }
    }
}
