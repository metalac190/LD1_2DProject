using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Inventory
{
    public event Action<int> CollectiblesChanged;
    public event Action<int> KeysChanged;

    public const int _collectibleMax = 9999;
    private int _collectibles = 0;
    public int Collectibles 
    {
        get => _collectibles;
        set
        {
            value = Mathf.Clamp(value, 0, _collectibleMax);
            if(value != _collectibles)
            {
                CollectiblesChanged?.Invoke(value);
            }
            _collectibles = value;
        }
    }

    public const int _keyMax = 9999;
    private int _keys = 0;
    public int Keys
    {
        get => _keys;
        set
        {
            value = Mathf.Clamp(value, 0, _keyMax);
            if (value != _keys)
            {
                KeysChanged?.Invoke(value);
            }
            _keys = value;
        }
    }
}
