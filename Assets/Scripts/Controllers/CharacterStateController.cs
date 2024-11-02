using UnityEngine.Assertions;
using UnityEngine;
using UnityEngine.Pool;

public enum CharacterState
{
    Idle,
    Moving,
    Attacking,
    Hit,
    Dying
}

[RequireComponent(typeof(HealthComponent))]
public class CharacterStateController : MonoBehaviour
{
    [SerializeField] private StatsProvider _statsProvider;
    [SerializeField] private CharacterMovementController _movementController;
    [SerializeField] private CharacterAttackController _attackController;
    [SerializeField] private CharacterViewController _viewController;
    [SerializeField] private TargetDataProvider _targetDataProvider;
    [SerializeField] private HealthComponent _healthComponent;
    [SerializeField] private bool _ignoreCollision = true;

    private CharacterState _characterState = CharacterState.Idle;
    private WeaponTypeConfig _currentWeaponTypeConfig;

    private ObjectPool<CharacterStateController> _characterPool;

    public HealthComponent HealthComponent { get { return _healthComponent; } }

    void Awake()
    {
        Initialize();
    }

    private void OnEnable()
    {
        Initialize();
    }

    public void Initialize()
    {
        Assert.IsNotNull(_viewController, "character view controller not assigned");

        if (_movementController != null)
        {
            _movementController.OnIdle += OnIdle;
            _movementController.OnMoving += OnMoving;
        }

        if (_attackController != null)
        {
            _attackController.OnAttack += OnAttack;
            _attackController.OnInterruptedAttack += OnInterruptedAttack;
        }
        SetWeaponType();
        InitializeMovementController();
        InitializeAttackController();
        _healthComponent.OnHitAction += OnHit;
        _healthComponent.OnDieAction += OnDieAction;
        _healthComponent.Reset();
    }

    void OnHit()
    {
        _viewController.OnHitAction();
        _characterState = CharacterState.Hit;
    }

    void OnDieAction()
    {
        _viewController.OnDieAction();
        _characterState = CharacterState.Dying;
    }

    public void OnDeath()
    {
        _healthComponent.OnDeath();
        _characterPool.Release(this);
    }

    protected void SetWeaponType()
    {
        Assert.IsNotNull(_statsProvider, "Stats Provider must be set");
        _statsProvider.Initialize();
        _statsProvider.GetStatsProviderEventHandler().OnWeaponChanged += OnWeaponChanged;
        _currentWeaponTypeConfig = _statsProvider.GetCurrentWeaponTypeConfig();
    }

    void OnWeaponChanged(WeaponTypeConfig weapon)
    {
        _currentWeaponTypeConfig = _statsProvider.GetCurrentWeaponTypeConfig();
        if (_movementController != null)
        {
            _movementController.UpdateMovementStats(_currentWeaponTypeConfig.WeaponConfig().MovementSpeed(),
                                                    _currentWeaponTypeConfig.WeaponConfig().AttackRange());
        }
        if (_attackController != null)
        {
            _attackController.UpdateAttackStats(_currentWeaponTypeConfig.WeaponConfig().AttackInterval(),
                                                _currentWeaponTypeConfig.WeaponConfig().AttackRange(),
                                                _currentWeaponTypeConfig.WeaponConfig().AttackDamage());
        }
        _viewController.SetWeaponType(_currentWeaponTypeConfig.WeaponType());
    }

    private void InitializeMovementController()
    {
        if (_movementController != null)
        {
            _movementController.Initialize(_targetDataProvider, _currentWeaponTypeConfig.WeaponConfig().MovementSpeed(),
                                                                _currentWeaponTypeConfig.WeaponConfig().AttackRange());
        }
    }

    private void InitializeAttackController()
    {
        if (_attackController != null)
        {
            _attackController.Initialize(_targetDataProvider, 
                                         _currentWeaponTypeConfig.WeaponConfig().AttackInterval(), 
                                         _currentWeaponTypeConfig.WeaponConfig().AttackRange(), 
                                         _currentWeaponTypeConfig.WeaponConfig().AttackDamage());
        }
    }

    void OnDestroy()
    {
        StopEvents();
    }

    private void OnDisable()
    {
        StopEvents();
    }

    public void StopEvents()
    {
        if (_movementController != null)
        {
            _movementController.OnIdle -= OnIdle;
            _movementController.OnMoving -= OnMoving;
        }

        if (_attackController != null)
        {
            _attackController.OnAttack -= OnAttack;
            _attackController.OnInterruptedAttack -= OnInterruptedAttack;
        }
        _healthComponent.OnHitAction -= OnHit;
        _healthComponent.OnDieAction -= OnDieAction;
        if (_statsProvider != null)
        {
            _statsProvider.GetStatsProviderEventHandler().OnWeaponChanged -= OnWeaponChanged;
        }
    }

    private bool HasValidData()
    {
        return _viewController != null;
    }

    void OnIdle()
    {
        if (!HasValidData() || _characterState == CharacterState.Attacking || _characterState == CharacterState.Hit || _characterState == CharacterState.Dying)
        {
            return;
        }
        if (_attackController != null)
        {
            _attackController.OnStartAttack();
        }
        _viewController.OnIdle();
        _characterState = CharacterState.Idle;
    }

    void OnMoving(float movementSpeed)
    {
        if (!HasValidData())
        {
            return;
        }
        if (_attackController != null)
        {
            _attackController.OnStopAttack(movementSpeed);
        }
        _viewController.OnMoving(movementSpeed);
        _characterState = CharacterState.Moving;
    }

    void OnAttack()
    {
        if (!HasValidData())
        {
            return;
        }
        OnAttackExecute();
        _characterState = CharacterState.Attacking;
    }

    private void OnAttackExecute()
    {
        _viewController.OnAttack(_currentWeaponTypeConfig.WeaponConfig().AttackAnimationSpeed());
    }

    void OnInterruptedAttack()
    {
        if (!HasValidData())
        {
            return;
        }
        _viewController.OnIdle();
        _characterState = CharacterState.Idle;
    }

    public void OnAttackEvent()
    {
        if (_attackController == null || _characterState == CharacterState.Moving)
        {
            return;
        }
        _attackController.OnAttackExecuted();
    }

    void OnTriggerEnter(Collider other)
    {
        if (_ignoreCollision)
        {
            return;
        }
        var weaponData = other.gameObject.GetComponent<WeaponData>();
        if (weaponData == null)
        {
            return;
        }

        _statsProvider.SetCurrentWeaponConfigByType(weaponData.WeaponType);
        Destroy(other.gameObject);
    }

    public void SetObjectPool(ObjectPool<CharacterStateController> characterPool)
    {
        _characterPool = characterPool;
    }
}