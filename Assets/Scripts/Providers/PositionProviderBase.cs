using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public abstract class PositionProviderBase
{
    public virtual Vector3 GetPosition()
    {
        return Vector3.zero;
    }
}
