using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PhysicsHelper
{
    public static bool IsInLayerMask(GameObject gameObject, LayerMask layerMask)
    {
        return layerMask == (layerMask | (1 << gameObject.layer));
    }
}
