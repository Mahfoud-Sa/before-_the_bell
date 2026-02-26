using UnityEngine;
using TMPro;

public class CurrencyUI : MonoBehaviour
{
    public TextMeshProUGUI coinText; // ربط Text من الـ Canvas

    void Start()
    {
        if (CurrencyManager.Instance != null)
            CurrencyManager.Instance.OnCurrencyChanged += UpdateUI;

        UpdateUI(CurrencyManager.Instance != null ? CurrencyManager.Instance.currentCoins : 0);
    }

    private void OnDestroy()
    {
        if (CurrencyManager.Instance != null)
            CurrencyManager.Instance.OnCurrencyChanged -= UpdateUI;
    }

    private void UpdateUI(int amount)
    {
        if (coinText != null)
            coinText.text = amount.ToString();
    }
}
