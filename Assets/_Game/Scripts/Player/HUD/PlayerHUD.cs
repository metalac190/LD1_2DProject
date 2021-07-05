using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{
    [SerializeField]
    private Player _player;
    [SerializeField]
    private IconBar _healthBarGUI;
    [SerializeField]
    private Text _fragmentTextGUI;
    [SerializeField]
    private Text _keysTextGUI;
    [SerializeField]
    private Text _artifactsTextGUI;

    private Health _health;
    private Inventory _inventory;

    private void Awake()
    {
        // dependencies
        _health = _player.Health;
        _inventory = _player.Inventory;
        // setup
        _healthBarGUI.CreateIcons(_health.HealthMax);
    }

    private void Start()
    {
        // initial values
        _healthBarGUI.FillIcons(_health.HealthCurrent);
        _fragmentTextGUI.text = _inventory.Fragments.ToString();
        _keysTextGUI.text = _inventory.Keys.ToString();
    }

    private void OnEnable()
    {
        _health.HealthChanged += OnChangedHealth;
        _inventory.FragmentsChanged += OnFragmentsChanged;
        _inventory.KeysChanged += OnKeysChanged;
        _inventory.ArtifactsChanged += OnArtifactsChanged;
    }

    private void OnDisable()
    {
        _health.HealthChanged -= OnChangedHealth;
        _inventory.FragmentsChanged -= OnFragmentsChanged;
        _inventory.KeysChanged -= OnKeysChanged;
        _inventory.ArtifactsChanged -= OnArtifactsChanged;
    }

    private void OnChangedHealth(int newHealth)
    {
        _healthBarGUI.FillIcons(newHealth);
    }

    private void OnFragmentsChanged(int newCollectibles)
    {
        _fragmentTextGUI.text = newCollectibles.ToString();
    }

    private void OnKeysChanged(int newKeysAmount)
    {
        _keysTextGUI.text = newKeysAmount.ToString();
    }

    private void OnArtifactsChanged(int newArtifactAmount)
    {
        _artifactsTextGUI.text = newArtifactAmount.ToString();
    }
}
