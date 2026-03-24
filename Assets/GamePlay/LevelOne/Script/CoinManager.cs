using UnityEngine;
using TMPro;

public class CoinManager : MonoBehaviour
{
    public static CoinManager Instance;

    public TMP_Text coinText;
    public int currentCoins = 0;

    private string coinSaveKey = "PlayerSavedCoins";

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // إبقاء المدير عند تغيير المشاهد
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // نبدأ دائماً من 0 عند بداية اللعبة
        currentCoins = 0;
        UpdateUI();
    }

    public void AddCoins(int amount)
    {
        currentCoins += amount;
        UpdateUI();
    }

    void UpdateUI()
    {
        if (coinText != null)
        {
            coinText.text = " " + currentCoins.ToString();
        }
    }

    // دالة جديدة لحفظ التقدم (اختياري)
    public void SaveProgress()
    {
        PlayerPrefs.SetInt(coinSaveKey, currentCoins);
        PlayerPrefs.Save();
        Debug.Log("تم حفظ التقدم: " + currentCoins + " قطعة");
    }

    // دالة جديدة لتحميل التقدم (اختياري)
    public void LoadProgress()
    {
        currentCoins = PlayerPrefs.GetInt(coinSaveKey, 0);
        UpdateUI();
        Debug.Log("تم تحميل التقدم: " + currentCoins + " قطعة");
    }

    // دالة جديدة لإعادة التعيين إلى 0
    public void ResetCoins()
    {
        currentCoins = 0;
        UpdateUI();
        Debug.Log("تم إعادة تعيين القطع إلى 0");
    }
}