using System;

public class View : IDisposable
{
    public ReactiveProperty<int> coins = new ReactiveProperty<int>();

    public ReactiveProperty<int> upgradePrice = new ReactiveProperty<int>();

    public ReactiveProperty<int> pistolLevel = new ReactiveProperty<int>();

    public ReactiveProperty<float> bombCooldown = new ReactiveProperty<float>();

    public void Dispose()
    {
        coins.Dispose();
        upgradePrice.Dispose();
        pistolLevel.Dispose();
        bombCooldown.Dispose();
    }
}
