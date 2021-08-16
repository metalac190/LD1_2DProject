using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SoundSystem;

/// <summary>
/// For this solution we're not going to use Unity Physics, and instead attempt to
/// do all of our collision checks using periodic Overlap checks. This is mainly for optimization
/// and because our projectiles shoudln't need to interact with colliders much apart
/// from applying damage and occasionally being destroyed by things
/// </summary>

public abstract class ProjectileBase : MonoBehaviour
{
    public abstract void Move(MovementKM movement);

    [Header("Dependencies")]
    [SerializeField]
    private MovementKM _movement;
    [SerializeField]
    private HitVolume _hitVolume;

    [Header("Properties")]
    [SerializeField]
    private bool _destroyOnImpact = true;

    [Header("FX")]
    [SerializeField]
    private ParticleSystem _impactVFX;
    [SerializeField]
    private SFXOneShot _impactSFX;

    private void OnEnable()
    {
        _hitVolume.Hit += OnHit;
    }

    private void OnDisable()
    {
        _hitVolume.Hit -= OnHit;

    }

    private void FixedUpdate()
    {
        // since this object does not 'own' a collider, we can move in Update
        Move(_movement);
    }

    private void OnHit(GameObject gameObject)
    {
        PlayFX();
        Impact();
    }

    protected virtual void PlayFX()
    {
        if(_impactVFX != null)
        {
            ParticleSystem vfx = Instantiate(_impactVFX, 
                transform.position, Quaternion.identity);
            vfx.Play();
        }
        if(_impactSFX != null)
        {
            _impactSFX.PlayOneShot(transform.position);
        }
    }

    protected virtual void Impact()
    {
        if (_destroyOnImpact)
        {
            Destroy(gameObject);
        }
    }
}
