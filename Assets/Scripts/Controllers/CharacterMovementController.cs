using UnityEngine.Assertions;
using UnityEngine;
using System;

[RequireComponent(typeof(CharacterRotationController))]
public class CharacterMovementController : MonoBehaviour
{
    [SerializeReference] MovementDirectionProvider _directionProvider;

    private Vector3 _calculatedMoveDirection = Vector3.zero;
    private CharacterRotationController _rotationController;
    private float _currentMovementSpeed = 0.1f;
    private bool _initialized = false;

    public event Action<float> OnMoving;
    public event Action OnIdle;

    public void Initialize(TargetDataProvider targetDataProvider, float movementSpeed, float movementRange)
    {
        Assert.IsNotNull(_directionProvider, "CharacterMovementController must have set a MovementDirectionProvider");
        _directionProvider.Initialize(targetDataProvider);
        UpdateMovementStats(movementSpeed, movementRange);
        _rotationController = GetComponent<CharacterRotationController>();
        _initialized = true;
    }

    public void UpdateMovementStats(float movementSpeed, float movementRange)
    {  
        _currentMovementSpeed = movementSpeed;
        _directionProvider.UpdateMovementRange(movementRange);
    }

    void OnDisable()
    {
        _initialized = false;
    }

    void Update()
    {
        if (!_initialized)
        {
            return;
        }

        _directionProvider.UpdateCacheInfo();
        if (_directionProvider.IsOnIdle())
        {
            OnIdleExecute();
            return;
        }

        transform.position += _directionProvider.GetMovementDirection() * _currentMovementSpeed * Time.deltaTime;

        OnMovingExecute(_directionProvider.GetMovementDirection().magnitude);

        _rotationController.RotateTowards(_directionProvider.GetMovementDirection());
    }

    private void OnIdleExecute()
    {
        OnIdle?.Invoke();
    }

    private void OnMovingExecute(float direction)
    {
        OnMoving?.Invoke(direction);
    }
}
