using System;
using UnityEngine;

public class WeaponViewController : MonoBehaviour
{
    [System.Serializable]
    public struct WeaponViewData
    {
        public WeaponType weaponType;
        public MeshRenderer weaponMesh;
    }

    public WeaponViewData[] WeaponViewDataList;

    public void SetWeaponView(WeaponType weaponType)
    {
        foreach (var weapon in WeaponViewDataList)
        {
            weapon.weaponMesh.enabled = weaponType == weapon.weaponType;
        }
    }
}
