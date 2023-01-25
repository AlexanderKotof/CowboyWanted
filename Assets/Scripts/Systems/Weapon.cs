using System;

[Serializable]
public class Weapon
{
    public float damage = 5;

    public int level { get; private set; }

    public Weapon(Weapon weapon)
    {
        damage = weapon.damage;
        level = weapon.level;
    }

    public void Upgrade()
    {
        damage *= 2;
        level++;
    }
}
