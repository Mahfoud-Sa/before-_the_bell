using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float gameTime = 60f;
    private float currentTime;
    private bool gameEnded = false;

    void Start()
    {
        currentTime = gameTime;
        Time.timeScale = 1f;
    }

    void Update()
    {
        if (gameEnded)
            return;

        currentTime -= Time.deltaTime;

        if (currentTime <= 0f)
        {
            GameOver();
        }
    }

    public void GameOver()
    {
        if (gameEnded) return;

        gameEnded = true;
        EndGameUI.Instance.ShowGameOver();
    }

    public void WinGame()
    {
        if (gameEnded) return;

        gameEnded = true;
        EndGameUI.Instance.ShowWin();
    }
}