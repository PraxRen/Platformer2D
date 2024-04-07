using System;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }
    public int Coins { get; private set; }

    public event Action<int> UpdatedCoins;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void AddCoin()
    {
        Coins++;
        UpdatedCoins?.Invoke(Coins);
    }
}