using System;
using UnityEngine;

public class CoinCounter : MonoBehaviour
{
    public event Action<int> UpdatedCoins;

    public int Coins { get; private set; }

    public void AddCoins(int count)
    {
        if (count < 0)
            throw new ArgumentOutOfRangeException(nameof(count));

        Coins += count;
        UpdatedCoins?.Invoke(Coins);
    }
}