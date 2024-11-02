using System;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponListConfig", menuName = "ScriptableObjects/WeaponListConfig")]
public class WeaponListConfig : ScriptableObject
{
    public WeaponTypeConfig[] WeaponConfig;

    public WeaponTypeConfig GetWeaponTypeConfigByType(WeaponType weaponType)
    {
        foreach (var weaponConfig in WeaponConfig)
        {
            if (weaponConfig.WeaponType() == weaponType)
            {
                return weaponConfig;
            }
        }
        return null;
    }
}
