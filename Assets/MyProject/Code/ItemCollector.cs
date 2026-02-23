using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class ItemCollector : MonoBehaviour
{
    private int cherries = 0;

    [SerializeField] private TextMeshProUGUI cherriesText;




    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Cherry"))
        {

            Destroy(collision.gameObject);
            cherries++;
            cherriesText.text = "" + cherries;
        }
    }

    public bool CheckWinStatus()
    {
        if (cherries == 5) return true;
        else return false;
        
    }
}