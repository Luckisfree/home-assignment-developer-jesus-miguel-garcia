using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovementDirectionProvider : MonoBehaviour
{
    protected Vector3 _movementDirection = Vector3.zero;

    public abstract void Initialize(TargetDataProvider targetDataProvider);

    public abstract Vector3 GetMovementDirection();

    public abstract bool IsOnIdle();

    public abstract void UpdateCacheInfo();

    public abstract void UpdateMovementRange(float movementRange);
}
