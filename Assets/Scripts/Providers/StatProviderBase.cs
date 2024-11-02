using System;
using UnityEngine;

public class StatProviderEventHandler
{
    public event Action<WeaponTypeConfig> OnWeaponChanged;
    public void OnWeaponChangeExecute(WeaponTypeConfig weapon)
    {
        OnWeaponChanged?.Invoke(weapon);
    }
}

public abstract class StatProviderBase : MonoBehaviour
{
    protected StatProviderEventHandler _statProviderEventHandler = new StatProviderEventHandler();

    public abstract void Initialize();
   
    public abstract WeaponTypeConfig GetCurrentWeaponTypeConfig();

    public abstract void SetCurrentWeaponConfigByType(WeaponType weaponType);

    public abstract StatProviderEventHandler GetStatProviderEventHandler();
}
