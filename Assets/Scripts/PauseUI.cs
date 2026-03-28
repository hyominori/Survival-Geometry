using UnityEngine;

public class PauseUI : MonoBehaviour
{
    public GameObject pausePanel;

    public MenuUI menuUI;

    private bool isPaused = false;

    public void TogglePause()
    {
        if (isPaused)
            Resume();
        else
            Pause();
    }

    public void Pause()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void Resume()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        pausePanel.SetActive(false);

        GameManager.Instance.ResetGame();
        GameManager.Instance.StartStage(0);
    }

    public void BackToMenu()
    {
        Time.timeScale = 0f;
        pausePanel.SetActive(false);

        menuUI.BackToMenu(); // gọi về menu
    }
}