using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SoundSystem;

public class DestroyOnKill : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField]
    private Health _health;

    [Header("Optional")]
    [SerializeField]
    private ParticleSystem _killParticlesPrefab;
    [SerializeField]
    private SFXOneShot _killSFX;
    [SerializeField]
    private GameObject _spawnPrefabOnDeath;

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
        PlayVFX();
        SpawnObject();

        Destroy(gameObject);
    }

    private void PlayVFX()
    {
        if (_killParticlesPrefab != null)
        {
            ParticleSystem killParticles = Instantiate(_killParticlesPrefab,
                transform.position, Quaternion.identity);
            killParticles.Play();
        }
        if (_killSFX != null)
        {
            _killSFX.PlayOneShot(transform.position);
        }
    }

    private void SpawnObject()
    {
        if(_spawnPrefabOnDeath != null)
        {
            Debug.Log("Spawn Object");
            Instantiate(_spawnPrefabOnDeath, transform.position, Quaternion.identity);
        }
    }
}
