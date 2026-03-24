using UnityEngine;

public class WinPanelUI : MonoBehaviour
{
    public GameObject[] stars; // Drag Star1, Star2, Star3 here

    [ContextMenu("Test ShowPanel")]

    void Awake()
    {
        int currentStars = GameManager.Instance.currentStars;
        Debug.Log("ShowPanel called on: " + gameObject.name + " with " + currentStars + " stars");
    for (int i = 0; i < stars.Length; i++)
    {
        Debug.Log("Activating star: " + stars[i].name);
        stars[i].SetActive(i < currentStars);
    }
    gameObject.SetActive(true);
    }
//     public void ShowPanel()
// {
//     Debug.Log("ShowPanel called on: " + gameObject.name);
//     for (int i = 0; i < stars.Length; i++)
//     {
//         Debug.Log("Activating star: " + stars[i].name);
//         stars[i].SetActive(i < 2); // hardcoded for test
//     }
//     gameObject.SetActive(true);
// }
}