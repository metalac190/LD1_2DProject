using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SoundSystem;

[RequireComponent(typeof(Collider2D))]
public abstract class Pickup : MonoBehaviour
{
    // this is our template method. Subclasses must implement
    protected abstract void OnPickup(GameObject collector);

    [Header("Feedback")]
    [SerializeField] SFXOneShot _pickupSFX = null;
    [SerializeField] ParticleSystem _particlePrefab = null;

    // Reset gets called whenever you add a component to an object
    private void Reset()
    {
        // set isTrigger in the Inspector, just in case Designer forgot
        Collider2D collider = GetComponent<Collider2D>();
        collider.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // guard clause
        GameObject collector = other.gameObject;
    
        // found the player! call our abstract method and supporting feedback
        OnPickup(collector);

        if (_pickupSFX != null)
        {
            _pickupSFX.PlayOneShot(transform.position);
        }

        if (_particlePrefab != null)
        {
            SpawnParticle(_particlePrefab);
        }

        Disable();
    }

    void SpawnAudio(AudioClip pickupSFX)
    {
        AudioSource.PlayClipAtPoint(pickupSFX, transform.position);
    }

    void SpawnParticle(ParticleSystem pickupParticles)
    {
        ParticleSystem newParticles =
            Instantiate(pickupParticles, transform.position, Quaternion.identity);

        newParticles.Play();
    }

    // allow override in case subclass wants to object pool, etc.
    protected virtual void Disable()
    {
        gameObject.SetActive(false);
    }
}
