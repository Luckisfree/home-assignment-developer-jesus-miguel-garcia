using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickInputMovementDirectionProvider : MovementDirectionProvider
{
    [SerializeField] Joystick _joystick;

    public override void Initialize(TargetDataProvider _)
    {
    }

    public override Vector3 GetMovementDirection()
    {
        return _movementDirection;
    }

    public override void UpdateCacheInfo()
    {
        _movementDirection.Set(_joystick.Direction.x, transform.position.y, _joystick.Direction.y);
    }

    public override bool IsOnIdle()
    {
        return _joystick.Direction.sqrMagnitude == 0;
    }

    public override void UpdateMovementRange(float movementRange)
    {

    }
}
