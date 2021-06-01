using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct AttackDetails
{
    public readonly int Damage;
    public readonly float Knockback;
    public readonly GameObject Source;

    public AttackDetails(int damage, float knockback, GameObject source)
    {
        this.Damage = damage;
        this.Knockback = knockback;
        this.Source = source;
    }
}
