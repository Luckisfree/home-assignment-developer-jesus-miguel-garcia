using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsProvider : MonoBehaviour
{
    [SerializeReference] StatProviderBase _statsProvider;

    public void Initialize()
    {
        _statsProvider.Initialize();
    }

    public WeaponTypeConfig GetCurrentWeaponTypeConfig()
    {
        return _statsProvider.GetCurrentWeaponTypeConfig();
    }

    public StatProviderEventHandler GetStatsProviderEventHandler()
    {
        return _statsProvider.GetStatProviderEventHandler();
    }

    public void SetCurrentWeaponConfigByType(WeaponType weaponType)
    {
        _statsProvider.SetCurrentWeaponConfigByType(weaponType);
    }
}
