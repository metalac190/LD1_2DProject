using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockbackOtherOnTouch : MonoBehaviour
{
    [SerializeField] private float _knockbackCooldown = .2f;
    [SerializeField] private float _knockbackAmount = 7.5f;
    [SerializeField] private float _knockbackDuration = .1f;

    [SerializeField] private Transform _touchDamageCheck;
    [SerializeField] private float _touchDamageWidth;
    [SerializeField] private float _touchDamageHeight;

    float _timeAtLastTouch = 0;

    private Vector2 _touchDamageBotLeft;
    private Vector2 _touchDamageTopRight;

    /*
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // if we've gone long enough without being damaged
        if (Time.time >= _timeAtLastTouch + _knockbackCooldown)
        {
            // if we're only looking for player, check
            if (_onlyAffectPlayer)
            {
                // if it's the player, apply damage/knockback and reset cooldown
                if (collision.gameObject.CompareTag("Player"))
                {
                    
                    ReceiveKnockback knockback = collision.gameObject.GetComponent<ReceiveKnockback>();
                    if (knockback != null)
                    {
                        Debug.Log("Apply Knockback to player");
                        knockback.Knockback(_knockbackAmount, _knockbackDuration, transform);
                    }

                    _timeAtLastTouch = Time.time;
                }
            }

            // otherwise apply knockback if it can receive it
            else
            {
                Debug.Log("Apply Knockback to object");
                ReceiveKnockback knockback = collision.gameObject.GetComponent<ReceiveKnockback>();
                if (knockback != null)
                {
                    knockback.Knockback(_knockbackAmount, _knockbackDuration, transform);
                }

                _timeAtLastTouch = Time.time;
            }
        }
    }
    */

    private void FixedUpdate()
    {
        CheckTouchDamage();
    }

    private void CheckTouchDamage()
    {
        // if we've gone long enough without being damaged
        if (Time.time >= _timeAtLastTouch + _knockbackCooldown)
        {
            // create bounds
            _touchDamageBotLeft.Set(_touchDamageCheck.position.x - (_touchDamageWidth / 2),
                _touchDamageCheck.position.y - (_touchDamageHeight / 2));
            _touchDamageTopRight.Set(_touchDamageCheck.position.x + (_touchDamageWidth / 2),
                _touchDamageCheck.position.y + (_touchDamageHeight / 2));
            // test with bounds
            Collider2D hit = Physics2D.OverlapArea(_touchDamageBotLeft, _touchDamageTopRight);
            // if it's the player, apply damage and knockback
            if (hit.CompareTag("Player"))
            {
                ReceiveKnockback knockback = hit.GetComponent<ReceiveKnockback>();
                if (knockback != null)
                {
                    Debug.Log("Apply Knockback to player");
                    knockback.Knockback(_knockbackAmount, _knockbackDuration, transform);
                }

                _timeAtLastTouch = Time.time;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Vector2 botLeft = new Vector2(_touchDamageCheck.position.x - (_touchDamageWidth / 2),
            _touchDamageCheck.position.y - (_touchDamageHeight / 2));
        Vector2 botRight = new Vector2(_touchDamageCheck.position.x + (_touchDamageWidth / 2),
            _touchDamageCheck.position.y - (_touchDamageHeight / 2));
        Vector2 topRight = new Vector2(_touchDamageCheck.position.x + (_touchDamageWidth / 2),
            _touchDamageCheck.position.y + (_touchDamageHeight / 2));
        Vector2 topLeft = new Vector2(_touchDamageCheck.position.x - (_touchDamageWidth / 2),
            _touchDamageCheck.position.y + (_touchDamageHeight / 2));

        Gizmos.color = Color.red;
        Gizmos.DrawLine(botLeft, botRight);
        Gizmos.DrawLine(botRight, topRight);
        Gizmos.DrawLine(topRight, topLeft);
        Gizmos.DrawLine(topLeft, botLeft);
    }
}
