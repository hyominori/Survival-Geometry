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

        GameManager.Instance.RestartGame();
    }

    public void BackToMenu()
    {
        Time.timeScale = 1f;
        pausePanel.SetActive(false);

        if (menuUI != null)
        {
            GameManager.Instance.ClearStageObjects();
            menuUI.BackToMenu();
        }
        else
            Debug.LogError("MenuUI chưa được gán!");
    }
}