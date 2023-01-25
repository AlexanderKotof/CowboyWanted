using System;

public class CoinsSystem : IDisposable
{
    public int Coins => _coins;
    private int _coins;

    public event Action<int> CoinsUpdated;

    public bool HasCoins(int value)
    {
        return _coins >= value;
    }

    public bool SpendCoins(int count)
    {
        if (_coins < count)
            return false;

        _coins -= count;

        CoinsUpdated?.Invoke(_coins);

        return true;
    }

    public void AddCoins(int value)
    {
        _coins += value;
        CoinsUpdated?.Invoke(_coins);
    }

    public void Dispose()
    {
        CoinsUpdated = null;
    }
}
