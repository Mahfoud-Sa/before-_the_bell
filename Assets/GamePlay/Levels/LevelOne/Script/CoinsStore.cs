using System;
using TMPro;
using UnityEngine;

public class CoinsStore : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI coins;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        coins.text = CurrencyManager.Instance.currentCoins.ToString();
    }
}
