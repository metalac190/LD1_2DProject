using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SoundSystem;

public class BounceZone : MonoBehaviour
{
    [SerializeField]
    private Vector2 _bounceDirection = new Vector2(0, 1);
    [SerializeField]
    private float _bounceAmount = 20;
    [SerializeField]
    private SFXOneShot _bounceSFX;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();
        if(player != null)
        {
            player.DashSystem.ReadyDash();

            player.Actor.Movement.Move(_bounceDirection * _bounceAmount);

            _bounceSFX?.PlayOneShot(player.transform.position);
        }
    }
}
