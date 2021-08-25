using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformStickyZone : MonoBehaviour
{
    [SerializeField]
    private MoveBetweenPoints _movingObject;

    private List<MovementKM> _passengers = new List<MovementKM>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        MovementKM movement = collision.gameObject.GetComponent<MovementKM>();
        if (movement != null)
        {
            _passengers.Add(movement);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        MovementKM movement = collision.gameObject.GetComponent<MovementKM>();
        if (movement != null)
        {
            _passengers.Remove(movement);
        }
    }

    private void FixedUpdate()
    {
        if(_passengers != null && _passengers.Count > 0)
        {
            foreach(MovementKM moveable in _passengers)
            {
                moveable.Move(_movingObject.Velocity, false);
            }
        }
    }
}
