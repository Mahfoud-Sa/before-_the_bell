using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelM : MonoBehaviour
{
    public int SceneN;
    public int SceneN1;
    public int SceneN2;
    public void MainMenu()
    {
        
            SceneManager.LoadScene(SceneN);
            //Time.timeScale = 1;
    }
    public void FirstScene()
    {

        SceneManager.LoadScene(SceneN1);
        Time.timeScale = 1;
    }
    public void SecoundScene()
    {

        SceneManager.LoadScene(SceneN2);
        Time.timeScale = 1;
    }

    public void PauseGame()
   {
        Time.timeScale = 0;
   }
    public void ResumeGame()
    {
        Time.timeScale = 1;
    }




}
