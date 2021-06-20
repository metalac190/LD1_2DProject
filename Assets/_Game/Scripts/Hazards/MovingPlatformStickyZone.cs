using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformStickyZone : MonoBehaviour
{
    [SerializeField]
    private MovingPlatform _movingPlatform;

    private List<MovementKM> _passengers = new List<MovementKM>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();
        if (player != null)
        {
            _passengers.Add(player.Movement);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();
        if (player != null)
        {
            _passengers.Remove(player.Movement);
        }
    }

    private void FixedUpdate()
    {
        if(_passengers != null && _passengers.Count > 0)
        {
            foreach(MovementKM moveable in _passengers)
            {
                moveable.Move(_movingPlatform.Velocity, false);
            }
        }
    }
}
