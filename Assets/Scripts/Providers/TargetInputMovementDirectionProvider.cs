using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetInputMovementDirectionProvider : MovementDirectionProvider
{
    private TargetDataProvider _targetDataProvider;
    private TargetInfo _targetInfo;
    private float _attackRange;

    public override void Initialize(TargetDataProvider targetDataProvider)
    {
        _targetDataProvider = targetDataProvider;
    }

    public override Vector3 GetMovementDirection()
    {
        return _movementDirection.normalized;
    }

    public override void UpdateCacheInfo()
    {
        _targetInfo = _targetDataProvider.GetClosestTargetFromPosition(transform.position);
        _movementDirection.Set(_targetInfo.direction.x, _targetInfo.target.transform.position.y, _targetInfo.direction.y);
    }

    public override bool IsOnIdle()
    {
        return _targetInfo.direction.sqrMagnitude <= _attackRange;
    }

    public override void UpdateMovementRange(float movementRange)
    {
        _attackRange = movementRange;
    }
}
