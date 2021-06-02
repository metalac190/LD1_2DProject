using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// In order to create a new dash ability, inhere from this, create the Scriptable Object, and make
/// sure that you call the override Completed event (Completed?.Invoke() ).
/// This will let the DashSystem know when the ability is over.
/// </summary>
public abstract class PlayerDashAbility : ScriptableObject
{
    // this method MUST be implemented when the dash officially starts
    public abstract event Action Completed;

    [Header("Base Settings")]
    [SerializeField] 
    private float _cooldown = 0;

    public float Cooldown
    {
        get => _cooldown;
        protected set { _cooldown = value; }
    }

    public virtual void OnInputPress(Player player)
    {

    }

    public virtual void OnUpdate(Player player)
    {

    }

    public virtual void OnFixedUpdate(Player player)
    {

    }

    // override and use this if ability needs to do special things when the button is released, after use
    // (charge dashes, for example)
    public virtual void OnInputRelease(Player player)
    {
        
    }
}
