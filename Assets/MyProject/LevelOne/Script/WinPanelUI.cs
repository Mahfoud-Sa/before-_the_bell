using UnityEngine;

public class WinPanelUI : MonoBehaviour
{
    public GameObject[] stars; // Drag Star1, Star2, Star3 here

    public void ShowPanel()
    {
        int starCount = GameManager.Instance.currentStars;

        // Activate correct number of stars
        for (int i = 0; i < stars.Length; i++)
        {
            stars[i].SetActive(i < starCount);
        }

        gameObject.SetActive(true);
    }
}