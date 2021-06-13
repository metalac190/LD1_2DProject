using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Inventory
{
    public event Action<int> FragmentsChanged;
    public event Action<int> ArtifactsChanged;
    public event Action<int> KeysChanged;

    public const int _fragmentsMax = 9999;
    private int _fragments = 0;
    public int Fragments 
    {
        get => _fragments;
        set
        {
            value = Mathf.Clamp(value, 0, _fragmentsMax);
            if(value != _fragments)
            {
                FragmentsChanged?.Invoke(value);
            }
            _fragments = value;
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

    public const int _artifactsMax = 999;
    private int _artifacts = 0;
    public int Artifacts
    {
        get => _artifacts;
        set
        {
            value = Mathf.Clamp(value, 0, _artifactsMax);
            if (value != _artifacts)
            {
                ArtifactsChanged?.Invoke(value);
            }
            _artifacts = value;
        }
    }
}
