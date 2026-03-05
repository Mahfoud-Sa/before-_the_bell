using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayButtonScript : MonoBehaviour
{
   // public StoryManager storyManager;
    private Button button;

    void Start()
    {
        button = GetComponent<Button>();
        button.interactable = true; // الزر غير قابل للضغط بالبداية
    }

    // void Update()
    // {
    //     if (storyManager != null && storyManager.storyCompleted)
    //     {
    //         button.interactable = true; // تفعيل الزر عندما تنتهي القصة
    //     }
    // }

    public void PlayGame()
    {
      //  if (storyManager != null )
         //   return; // لا يسمح بالدخول إذا لم تنته القصة
    Debug.Log("Play button pressed ");

        if (LoadingManager.Instance != null)
        {
            LoadingManager.Instance.LoadScene(2);
        }
        else
        {
            SceneManager.LoadScene(2);
        }
    }
}