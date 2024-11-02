using System.Collections;
using System.Collections.Generic;
using UnityEngine.Assertions;
using UnityEngine;
using System;

public class PlayerInventory : StatProviderBase
{
    [SerializeField] WeaponDataManager _weaponDataManager;

    private PlayerModel _playerModel;
    private WeaponTypeConfig _currentWeaponTypeConfig;

    public override void Initialize()
    {
        if (_weaponDataManager == null)
        {
            Assert.IsTrue(false, "WeaponDataManager not assigned on PlayerUpgradesController");
            return;
        }
        _playerModel = ServiceLocator.Instance.GetPlayerModel();
        var currentWeaponType = _playerModel.GetCurrentWeaponType();
        _currentWeaponTypeConfig = _weaponDataManager.GetWeaponTypeConfigByType(currentWeaponType);
        if(_currentWeaponTypeConfig == null )
        {
            Assert.IsTrue(false, "Unknown weapon assigned as current on PlayerModel");
        }
    }

    public override WeaponTypeConfig GetCurrentWeaponTypeConfig()
    {
        return _currentWeaponTypeConfig;
    }

    public override void SetCurrentWeaponConfigByType(WeaponType weaponType)
    {
        _currentWeaponTypeConfig = _weaponDataManager.GetWeaponTypeConfigByType(weaponType);
        GetStatProviderEventHandler().OnWeaponChangeExecute(_currentWeaponTypeConfig);
    }

    public override StatProviderEventHandler GetStatProviderEventHandler()
    {
        return _statProviderEventHandler;
    }
}
