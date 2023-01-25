using System;

public class WeaponSystem
{
    [Serializable]
    public class Settings
    {
        public Weapon weapon;
        public int[] upgradeCosts;
    }
    public Weapon Weapon { get; private set; }

    private Settings _settings;
    private CoinsSystem _coinsSystem;

    public event Action WeaponUpgraded;

    public WeaponSystem(Settings settings, CoinsSystem coinsSystem)
    {
        _settings = settings;
        _coinsSystem = coinsSystem;

        Weapon = new Weapon(_settings.weapon);
    }

    public bool CanUpgrade()
    {
        if (_settings.upgradeCosts.Length <= Weapon.level)
            return false;

        return _coinsSystem.HasCoins(GetUpgradeCost());
    }

    public int GetUpgradeCost()
    {
        return _settings.upgradeCosts.Length > Weapon.level ? _settings.upgradeCosts[Weapon.level] : 0;
    }

    public int GetWeaponLevel()
    {
        return Weapon.level;
    }

    public void UpgradeWeapon()
    {
        if (!CanUpgrade())
            return;

        if (!_coinsSystem.SpendCoins(GetUpgradeCost()))
            return;

        Weapon.Upgrade();

        WeaponUpgraded?.Invoke();
    }
}
