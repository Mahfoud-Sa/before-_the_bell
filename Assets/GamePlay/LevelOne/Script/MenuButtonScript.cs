using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtonScript : MonoBehaviour
{
 public void LoadMainMenu()
    {
        Time.timeScale = 1f; // make sure time is running
        SceneManager.LoadScene("MainMenu"); // put your main menu scene name here
    }
}
