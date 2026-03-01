using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButtonScript : MonoBehaviour
{
 public void PlayGame()
    {
       // LoadingManager.Instance.LoadScene("LevelOne");
        SceneManager.LoadScene(2);
        // OR if you want by index:
       //  LoadingManager.Instance.LoadScene("LevelOne 1");
    }
}
