using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("UI Elements")]
    public TextMeshProUGUI coinsText;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    // Call this whenever coins change
    public void UpdateCoinsText(int coins)
    {
        if (coinsText != null)
            coinsText.text = $"{coins}";
    }
}
