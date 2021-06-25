using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct HitData
{
    public readonly Transform Target;
    public readonly int Damage;
    public readonly Vector2 Direction;
    public readonly float KnockbackForce;
    public readonly float KnockbackDuration;

    public HitData(Transform target, int damage, 
        Vector2 direction, float knockbackForce, float knockbackDuration)
    {
        Target = target;
        Damage = damage;
        Direction = direction;
        KnockbackForce = knockbackForce;
        KnockbackDuration = knockbackDuration;
    }
}
