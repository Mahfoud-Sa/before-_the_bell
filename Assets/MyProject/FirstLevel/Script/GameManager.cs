using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Goat System")]
    public GoatRandomMovement goat;
    public Transform holdPosition;

    [Header("Timer System")]
    public float gameTime = 60f;
    private float currentTime;
    private bool gameEnded = false;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        currentTime = gameTime;
        Time.timeScale = 1f;
    }

    private void Update()
    {
        if (gameEnded)
            return;

        currentTime -= Time.deltaTime;

        if (currentTime <= 0f)
        {
            GameOver();
        }
    }

    // 🔘 Goat Button
    public void OnButtonPressed()
    {
        if (goat != null && holdPosition != null)
        {
            goat.Hold(holdPosition);
        }
    }

    // ❌ Game Over
    public void GameOver()
    {
        if (gameEnded) return;

        gameEnded = true;
        EndGameUI.Instance.ShowGameOver();
    }

    // 🏆 Win
    public void WinGame()
    {
        if (gameEnded) return;

        gameEnded = true;
        EndGameUI.Instance.ShowWin();
    }
}