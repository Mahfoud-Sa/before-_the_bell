using UnityEngine;
using System;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance { get; private set; }

    public int currentCoins = 0;
    public event Action<int> OnCurrencyChanged;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void AddCoins(int amount)
    {
        if (amount <= 0) return;
        currentCoins += amount;
        OnCurrencyChanged?.Invoke(currentCoins);
        Debug.Log("[CurrencyManager] Coins = " + currentCoins);
    }

    public void SetCoins(int amount)
    {
        currentCoins = amount;
        OnCurrencyChanged?.Invoke(currentCoins);
    }
}
