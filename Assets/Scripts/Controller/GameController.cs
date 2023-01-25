using System;

public class GameController : IDisposable
{
    public Action shoot;
    public Action upgradeWeapon;
    public Action throwBomb;

    public void Dispose()
    {
        shoot = null;
        upgradeWeapon = null;
        throwBomb = null;
    }
}
