using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PhysicsHelper
{
    /// <summary>
    /// Check if gameObject is within the specified layermask (checks with bitshifting)
    /// </summary>
    /// <param name="gameObject"></param>
    /// <param name="layerMask"></param>
    /// <returns></returns>
    public static bool IsInLayerMask(GameObject gameObject, LayerMask layerMask)
    {
        return layerMask == (layerMask | (1 << gameObject.layer));
    }

    /// <summary>
    /// Pass in a collider to cast and get contacts with other collisions. 
    /// </summary>
    /// <param name="collider"></param>
    /// <param name="direction"></param>
    /// <param name="distance"></param>
    /// <returns></returns>
    public static List<RaycastHit2D> CheckCollisions(Collider2D collider, Vector2 direction, float distance)
    {
        // setup collision filter to minimize contacts
        ContactFilter2D filter = new ContactFilter2D() { };
        filter.useTriggers = false;
        filter.SetLayerMask(Physics2D.GetLayerCollisionMask(collider.gameObject.layer));
        filter.useLayerMask = true;

        // prepare our hit storage
        RaycastHit2D[] hitBuffer = new RaycastHit2D[10];
        List<RaycastHit2D> hits = new List<RaycastHit2D>();

        // look for hits and store
        int hitCount = collider.Cast(direction, filter, hitBuffer, distance);
        for (int i = 0; i < hitCount; i++)
        {
            hits.Add(hitBuffer[i]);
        }

        return hits;
    }

    public static Vector2 ReverseVector(Vector2 start, Vector2 end)
    {
        Vector2 reverseVector = (start - end) * -1;
        reverseVector.Normalize();

        return reverseVector;
    }

    //returns -1 when to the left, 1 to the right, and 0 for forward/backward
    public static float RelativeDirection(Vector3 fwd, Vector3 targetDir, Vector3 up)
    {
        Vector3 perp = Vector3.Cross(fwd, targetDir);
        float dir = Vector3.Dot(perp, up);

        if (dir > 0.0f)
        {
            return 1.0f;
        }
        else if (dir < 0.0f)
        {
            return -1.0f;
        }
        else
        {
            return 0.0f;
        }
    }

    // returns 1 if relative is to the right of original, -1 if it's to the left
    public static float RelativeDirection(Vector2 originalposition, Vector2 relativePosition)
    {
        if (originalposition.x <= relativePosition.x)
        {
            return 1.0f;
        }
        else
        {
            return -1.0f;
        }
    }

    // returns -1 when left, 1 when right, 0 when aligned
    public static float AngleDir(Vector2 A, Vector2 B)
    {
        return -A.x * B.y + A.y * B.x;
    }
}
