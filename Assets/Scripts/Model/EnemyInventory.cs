using System.Collections;
using System.Collections.Generic;
using UnityEngine.Assertions;
using UnityEngine;
using System;

public class EnemyInventory : StatProviderBase
{
    [SerializeField] protected WeaponTypeConfig _defaultWeaponStats;

    public override void Initialize()
    {
        if(_defaultWeaponStats == null)
        {
            Assert.IsTrue(false, "Unknown weapon assigned as current on EnemyInventory");
        }
    }

    public override WeaponTypeConfig GetCurrentWeaponTypeConfig()
    {
        return _defaultWeaponStats;
    }

    public override void SetCurrentWeaponConfigByType(WeaponType weaponType)
    {
    }

    public override StatProviderEventHandler GetStatProviderEventHandler()
    {
        return _statProviderEventHandler;
    }
}
