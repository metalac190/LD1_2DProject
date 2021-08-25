using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SoundSystem;

[RequireComponent(typeof(Collider2D))]
public abstract class TriggerVolume : MonoBehaviour
{
    public abstract void TriggerEntered(Collider2D otherCollider);

    [SerializeField]
    private LayerMask _layersDetected;

    [Header("FX")]
    [SerializeField]
    private SFXOneShot _enteredSFX;
    [SerializeField]
    private ParticleSystem _enteredParticles;

    private Collider2D _collider;

    protected virtual void Awake()
    {
        // ensure it's marked as trigger
        _collider = GetComponent<Collider2D>();
        _collider.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D otherCollider)
    {
        // if we're not in the layer, return
        if (!PhysicsHelper.IsInLayerMask(otherCollider.gameObject, _layersDetected)) { return; }

        PlayFX();
        TriggerEntered(otherCollider);
    }

    private void OnTriggerExit2D(Collider2D otherCollider)
    {
        // if we're not in the layer, return
        if (!PhysicsHelper.IsInLayerMask(otherCollider.gameObject, _layersDetected)) { return; }

        TriggerExited(otherCollider);
    }

    protected virtual void TriggerExited(Collider2D otherCollider)
    {

    }

    private void PlayFX()
    {
        if (_enteredSFX != null)
            _enteredSFX.PlayOneShot(transform.position);

        if(_enteredParticles != null)
        {
            ParticleSystem newParticles =
                Instantiate(_enteredParticles, transform.position, Quaternion.identity);
            newParticles.Play();
        }  
    }
}
