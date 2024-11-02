using System;
using UnityEngine;

public class CharacterAttackController : MonoBehaviour
{
    private TargetDataProvider _targetDataProvider;
    private TargetInfo _targetInfo = new TargetInfo();
    private TimerController _attackTimer = new TimerController();
    private CharacterRotationController _rotationController;
    private CharactersData _charactersData;
    private Vector3 _cacheRotationDirection = Vector3.zero;
    private float _currentWeaponAttackInterval = 0.5f;
    private float _currentWeaponAttackRange = 5f;
    private float _currentWeaponAttackDamage = 50f;
    private bool _initialized = false;

    public event Action OnAttack;
    public event Action OnInterruptedAttack;

    public void Initialize(TargetDataProvider targetDataProvider, float attackInterval, float attackRange, float attackDamage)
    {
        _targetDataProvider = targetDataProvider;
        _charactersData = ServiceLocator.Instance.GetCharactersData();
        _charactersData.OnTargetDefeated += OnEnemyDefeated;
        InitializeTimer();
        _rotationController = GetComponent<CharacterRotationController>();
        _cacheRotationDirection = Vector3.zero;
        UpdateAttackStats(attackInterval, attackRange, attackDamage);
        _initialized = true;
    }

    public void UpdateAttackStats(float attackInterval, float attackRange, float attackDamage)
    {
        _currentWeaponAttackInterval = attackInterval;
        _currentWeaponAttackRange = attackRange;
        _currentWeaponAttackDamage = attackDamage;
    }

    private void InitializeTimer()
    {
        _attackTimer.Initialize(_currentWeaponAttackInterval);
        _attackTimer._OnTimeTickReached += OnTimeToAttack;
        _attackTimer.StopTimer();
        _attackTimer.ResetTimer();
    }

    void OnDestroy()
    {
        StopEvents();
    }

    void OnDisable()
    {
        StopEvents();
    }

    private void StopEvents()
    {
        _initialized = false;
        if (_attackTimer == null)
        {
            return;
        }
        _attackTimer.StopTimer();
        _attackTimer._OnTimeTickReached -= OnTimeToAttack;

        _charactersData.OnTargetDefeated -= OnEnemyDefeated;
    }

    private bool HasValidTarget()
    {
        return _targetInfo.HasValidTarget(); 
    }

    private void CacheClosestEnemy()
    {
        _targetInfo = _targetDataProvider.GetClosestTargetFromPosition(transform.position);
    }

    private bool IsClosestEnemyOnRange()
    {
        return HasValidTarget() && _targetInfo.direction.sqrMagnitude <= _currentWeaponAttackRange;
    }

    void Update()
    {
        if (!_initialized)
        {
            return;
        }
        if (_attackTimer == null || !_attackTimer.IsInitialized())
        {
            return;
        }

        _attackTimer.Update(Time.deltaTime);
    }

    public void OnStopAttack(float _)
    {
        ExecuteStopAttack();
    }

    private void ExecuteStopAttack()
    {
        _attackTimer.StopTimer();
        _attackTimer.ResetTimer();
    }

    private void OnEnemyDefeated(Vector3 _)
    {
        if (!HasValidTarget())
        {
            OnInterruptedAttack?.Invoke();
            ExecuteStopAttack();
        }
    }

    private bool IsTargetSet()
    {
        return _targetInfo.IsTargetSet();
    }

    public void OnStartAttack()
    {
        CacheClosestEnemy();
        if(!IsTargetSet())
        {
            return;
        }
        if(!IsClosestEnemyOnRange())
        {
            return;
        }
        TryLootAtEnemy();
        if (_attackTimer.IsRunning())
        {
            return;
        }
        _attackTimer.StartTimer();
    }

    void TryLootAtEnemy()
    {
        if (_rotationController != null)
        {
            var closestEnemyPosition = _targetInfo.target.transform.position;
            _cacheRotationDirection.Set(closestEnemyPosition.x - transform.position.x, transform.position.y, (closestEnemyPosition.z - transform.position.z));
            _rotationController.RotateTowards(_cacheRotationDirection);
        }
    }

    void OnTimeToAttack()
    {
        if (!IsClosestEnemyOnRange())
        {
            return;
        }
        TryLootAtEnemy();
        OnAttack?.Invoke();
    }

    private void HitTarget()
    {
        _targetInfo.target.HealthComponent.OnHit(_currentWeaponAttackDamage);
    }

    public void OnAttackExecuted()
    {
        OnInterruptedAttack?.Invoke();
        ExecuteStopAttack();
        if (!IsClosestEnemyOnRange())
        {
            return;
        }
        HitTarget(); ;
    }

    void OnDrawGizmos()
    {
        if(_targetDataProvider == null || !IsTargetSet() || _targetInfo.target.HealthComponent.IsDying())
        {
            return;
        }
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, _targetInfo.target.transform.position);
    }
}
