public class PlayerModel
{
    private WeaponType _currentWeaponType = WeaponType.CurvedSword;
    private int _currentKills = 0;

    public void Reset()
    {
        _currentWeaponType = WeaponType.CurvedSword;
        _currentKills = 0;
    }

    public WeaponType GetCurrentWeaponType()
    { 
        return _currentWeaponType; 
    }

    public int GetCurrentKills()
    {
        return _currentKills;
    }
}
