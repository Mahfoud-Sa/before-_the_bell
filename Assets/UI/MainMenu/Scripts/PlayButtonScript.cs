using UnityEngine;
using UnityEngine.UI;

public class PlayButtonScript : MonoBehaviour
{
    private Button button;

    void Start()
    {
        button = GetComponent<Button>();
        button.interactable = false; // الزر غير قابل للضغط بالبداية
    }

    public void PlayGame()
    {
        Debug.Log("Play button pressed");

        if (LoadingManager.Instance != null)
        {
            // Use the LoadingManager to load scene 2 with panel and animation
            LoadingManager.Instance.LoadScene(2);
        }
        else
        {
            // Fallback if LoadingManager is missing
            UnityEngine.SceneManagement.SceneManager.LoadScene(2);
        }
    }

    // Optional: you can add a method to enable button after story completes
    public void EnableButton()
    {
        if (button != null)
            button.interactable = true;
    }
}