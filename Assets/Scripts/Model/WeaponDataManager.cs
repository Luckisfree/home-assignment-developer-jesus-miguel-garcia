using UnityEngine;
using static WeaponListConfig;

public class WeaponDataManager : MonoBehaviour
{
    [SerializeField] WeaponListConfig _weaponListConfig;

    private CharactersData _charactersData;

    public WeaponListConfig WeaponListConfig()
    {
        return _weaponListConfig;
    }

    public WeaponTypeConfig GetWeaponTypeConfigByType(WeaponType type)
    {
        return _weaponListConfig.GetWeaponTypeConfigByType(type);
    }
}
