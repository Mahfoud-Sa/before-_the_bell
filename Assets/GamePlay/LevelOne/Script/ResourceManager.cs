using UnityEngine;
using TMPro;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager instance;
  //  public TextMeshProUGUI woodText; 
    private int woodAmount = 0;

    void Awake() { instance = this; }

   // void Start() { UpdateUI(); }

    public void AddWood(int amount)
    {
       // woodAmount += amount;
         // ✅ Add coins to GameManager
        if (GameManager.Instance != null)
        {
            GameManager.Instance.AddCoins(amount);
        }
      //  UpdateUI();
    }}

    // void UpdateUI()
    // {
    //     if (woodText != null)
    //         woodText.text = "" + woodAmount;
    // }}