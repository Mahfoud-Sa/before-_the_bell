using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButtonScript : MonoBehaviour
{
 public void PlayGame()
    {
        SceneManager.LoadScene("FirstLevel");
        
        // OR if you prefer using index:
        // SceneManager.LoadScene(1);
    }
}
