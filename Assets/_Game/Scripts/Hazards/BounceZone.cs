using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SoundSystem;

public class BounceZone : MonoBehaviour
{
    [SerializeField]
    private float _bounceAmount = 20;
    [SerializeField]
    private float _bounceDuration = .5f;
    [SerializeField]
    private SFXOneShot _bounceSFX;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();
        if(player != null)
        {
            player.DashSystem.ReadyDash();

            player.Movement.Push(transform.up, _bounceAmount, _bounceDuration);

            _bounceSFX?.PlayOneShot(player.transform.position);
        }
    }
}
