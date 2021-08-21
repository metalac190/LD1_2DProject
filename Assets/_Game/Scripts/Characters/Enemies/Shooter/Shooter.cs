using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : Enemy
{
    [Header("Shooter Settings")]
    [SerializeField]
    private float _shootRate = 1;
    [SerializeField]
    private ProjectileBase _projectile;

    public float ShootRate => _shootRate;
    public ProjectileBase Projectile => _projectile;

    public void Shoot(Transform target)
    {
        Debug.Log("Shoot!: " + target.position);
        //TODO consider object pooling here
        Vector2 shootDirection = target.position - transform.position;
        Debug.Log("Shoot Direction: " + shootDirection);
        //This assumes that your bullet sprite points to the right
        //Get the angle above the horizontal where the target is
        float angle = Vector3.Angle(Vector3.right, shootDirection);
        //This will always be positive, so lets flip the sign if it should be negative
        if (target.transform.position.y < transform.position.y) angle *= -1;
        //Create a rotation that will point towards the target
        Quaternion projectileDirection = Quaternion.AngleAxis(angle, Vector3.forward);

        GameObject projectile = Instantiate
            (_projectile.gameObject, transform.position, projectileDirection);

    }
}
