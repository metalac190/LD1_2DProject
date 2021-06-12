using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerHUD : MonoBehaviour
{
    [SerializeField]
    private Player _player;
    [SerializeField]
    private SimpleFillBar _healthBarGUI;
    [SerializeField]
    private TextMeshProUGUI _collectibleTextGUI;
    [SerializeField]
    private TextMeshProUGUI _keysTextGUI;

    private Health _health;
    private Inventory _inventory;

    private void Awake()
    {
        _health = _player.Health;
        _inventory = _player.Inventory;

        _healthBarGUI.SetScale(_health.HealthCurrent, _health.HealthMax);
        _collectibleTextGUI.text = _inventory.Collectibles.ToString();
        _keysTextGUI.text = _inventory.Keys.ToString();
    }

    private void OnEnable()
    {
        _health.HealthChanged += OnChangedHealth;
        _inventory.CollectiblesChanged += OnCollectiblesChanged;
        _inventory.KeysChanged += OnKeysChanged;
    }

    private void OnDisable()
    {
        _health.HealthChanged -= OnChangedHealth;
        _inventory.CollectiblesChanged += OnCollectiblesChanged;
        _inventory.KeysChanged -= OnKeysChanged;
    }

    private void OnChangedHealth(int newHealth)
    {
        _healthBarGUI.SetScale(newHealth, _health.HealthMax);
    }

    private void OnCollectiblesChanged(int newCollectibles)
    {
        _collectibleTextGUI.text = newCollectibles.ToString();
    }

    private void OnKeysChanged(int newKeysAmount)
    {
        _keysTextGUI.text = newKeysAmount.ToString();
    }
}
