using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPushable
{
    public void Push(Vector2 direction, float strength, float duration);
}
