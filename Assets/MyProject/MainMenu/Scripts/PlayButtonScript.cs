using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButtonScript : MonoBehaviour
{
    public void PlayGame()
    {
        if (LoadingManager.Instance != null)
        {
         
            LoadingManager.Instance.LoadScene(3);
        }
        else
        {
            SceneManager.LoadScene(3);
        }
    }
}