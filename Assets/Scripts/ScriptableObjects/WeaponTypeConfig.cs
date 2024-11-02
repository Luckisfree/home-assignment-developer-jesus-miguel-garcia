using System;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponTypeConfig", menuName = "ScriptableObjects/WeaponTypeConfig")]
public class WeaponTypeConfig : ScriptableObject
{
    [SerializeField] WeaponType _weaponType;
    [SerializeField] WeaponConfig _weaponConfig;
    [SerializeField] GameObject _weaponPrefab;

    public WeaponType WeaponType()
    { 
        return _weaponType; 
    }

    public WeaponConfig WeaponConfig()
    {
        return _weaponConfig;
    }

    public GameObject WeaponPrefab()
    { 
        return _weaponPrefab; 
    }
}

public enum WeaponType
{
    CurvedSword,
    GreatSword,
    LongSword,
    EnemyWeapon
}
