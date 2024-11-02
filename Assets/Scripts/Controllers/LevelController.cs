using UnityEngine.Pool;
using UnityEngine;

[RequireComponent(typeof(WeaponDataManager))]
public class LevelController : MonoBehaviour
{
    [Header("Enemies object pool parameters")]
    [SerializeField] int _defaultEnemyPoolSize;
    [SerializeField] int _maxEnemyPoolSize;

    [Header("Weapons after defeating an enemy spawning parameters")]
    [SerializeField] int _spawnWeaponProbabilities = 10;
    [SerializeField] Transform _floorTransform;
    [SerializeField] float _positionHeightOffset = 1f;

    [Header("Enemies spawning parameters")]
    [SerializeField] CharacterStateController _player;
    [SerializeField] GameObject _enemyGO;
    [SerializeField] TimerComponent _timerComponentToSpawn;

    [SerializeReference] PositionProviderBase _enemyPositionProvider;

    private CharactersData _charactersData;
    private WeaponDataManager _weaponDataManager;
    private WeaponListConfig _weaponListConfig; 
    private ObjectPool<CharacterStateController> _enemiesPool;

    //Const
    private const int _maximumProbabilities = 101;

    void Start()
    {
        _timerComponentToSpawn.TimerController.StartTimer();
        _timerComponentToSpawn.TimerController._OnTimeTickReached += OnTimeToSpawnReached;
        _timerComponentToSpawn.TimerController.ResetTimer();
        _weaponDataManager = GetComponent<WeaponDataManager>();
        _weaponListConfig = _weaponDataManager.WeaponListConfig();
        var serviceLocator = ServiceLocator.Instance;
        _charactersData = serviceLocator.GetCharactersData();
        _charactersData.SetEnemiesTarget(_player);
        _charactersData.OnTargetDefeated += OnEnemyDefeated;
        _enemiesPool = new ObjectPool<CharacterStateController>(CreateEnemy, OnSpawnEnemyFromPool, OnRecycleEnemyToPool, OnDestroyEnemy, true, _defaultEnemyPoolSize, _maxEnemyPoolSize);
    }

    void OnDestroy()
    {
        if(_timerComponentToSpawn == null || _timerComponentToSpawn.TimerController == null)
        {
            return;
        }
        _timerComponentToSpawn.TimerController._OnTimeTickReached -= OnTimeToSpawnReached;
        if (_charactersData == null)
        {
           return;
        }
        _charactersData.OnTargetDefeated -= OnEnemyDefeated;
    }

    void OnEnemyDefeated(Vector3 enemyPosition)
    {
        var weaponTypeConfig = TrySpawnWeapon();
        if (weaponTypeConfig == null)
        {
            return;
        }
        Instantiate(weaponTypeConfig.WeaponPrefab(), new Vector3(enemyPosition.x, _floorTransform.position.y + _positionHeightOffset, enemyPosition.z), Quaternion.identity);
    }

    private void OnTimeToSpawnReached()
    {
        if (!HasAllNeededData()) 
        {
            return;
        }

        _enemiesPool.Get();
    }

    private bool HasAllNeededData()
    {
        return _enemyPositionProvider != null && 
               _charactersData != null && 
               _timerComponentToSpawn != null && 
               _weaponDataManager != null &&
               _floorTransform != null;
    }

    public WeaponTypeConfig TrySpawnWeapon()
    {
        if (Random.Range(0, _maximumProbabilities) > _maximumProbabilities - _spawnWeaponProbabilities)
        {
            var randomIndex = Random.Range(0, _weaponListConfig.WeaponConfig.Length);
            return _weaponListConfig.WeaponConfig[randomIndex];
        }
        return null;
    }

    private CharacterStateController CreateEnemy()
    {
        var enemy = Instantiate(_enemyGO, _enemyPositionProvider.GetPosition(), Quaternion.identity);
        var enemyStateController = enemy.GetComponent<CharacterStateController>();
        enemyStateController.SetObjectPool(_enemiesPool);
        _charactersData.AddEnemy(enemyStateController);
        return enemyStateController;
    }

    private void OnSpawnEnemyFromPool(CharacterStateController characterStateController)
    {
        characterStateController.transform.position = _enemyPositionProvider.GetPosition();
        characterStateController.transform.rotation = Quaternion.identity;
        characterStateController.Initialize();
        characterStateController.gameObject.SetActive(true);
        _charactersData.AddEnemy(characterStateController);

    }

    private void OnRecycleEnemyToPool(CharacterStateController characterStateController)
    {
        characterStateController.gameObject.SetActive(false);
        _charactersData.RemoveEnemy(characterStateController);
        characterStateController.StopEvents();
    }

    private void OnDestroyEnemy(CharacterStateController characterStateController)
    {
        Destroy(characterStateController.gameObject);
    }
}
