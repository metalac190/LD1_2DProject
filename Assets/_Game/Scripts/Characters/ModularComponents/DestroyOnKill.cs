using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SoundSystem;

public class DestroyOnKill : MonoBehaviour
{
    [SerializeField]
    private Health _health;
    [SerializeField]
    private ParticleSystem _killParticlesPrefab;
    [SerializeField]
    private SFXOneShot _killSFX;

    private void OnEnable()
    {
        _health.Died.AddListener(OnDied);
    }

    private void OnDisable()
    {
        _health.Died.RemoveListener(OnDied);
    }

    private void OnDied()
    {
        if(_killParticlesPrefab != null)
        {
            ParticleSystem killParticles = Instantiate(_killParticlesPrefab,
                transform.position, Quaternion.identity);
            killParticles.Play();
        }
        if(_killSFX != null)
        {
            _killSFX.PlayOneShot(transform.position);
        }
        Debug.Log("Crawler Destroyed");
        Destroy(gameObject);
    }
}
