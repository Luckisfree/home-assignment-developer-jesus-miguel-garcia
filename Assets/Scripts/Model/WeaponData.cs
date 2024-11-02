using UnityEngine;

public class WeaponData : MonoBehaviour
{
    [SerializeField] WeaponType _weaponType;
    [SerializeField] TimerComponent _destroyTimerComponent;

    public WeaponType WeaponType
    { 
        get 
        { 
            return _weaponType; 
        }
    }

    void Start()
    {
        if(_destroyTimerComponent == null)
        {
            return;
        }

        _destroyTimerComponent.TimerController._OnTimeTickReached += OnTimerReached;
        _destroyTimerComponent.TimerController.StartTimer();
    }

    private void OnDestroy()
    {
        if (_destroyTimerComponent == null)
        {
            return;
        }

        _destroyTimerComponent.TimerController._OnTimeTickReached -= OnTimerReached;
        _destroyTimerComponent.TimerController.StopTimer();
    }

    private void OnTimerReached()
    {
        Destroy(gameObject);
    }
}
