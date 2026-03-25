using UnityEngine;

public class MenuUI : MonoBehaviour
{
    public GameObject mainMenuPanel;
    public GameObject tutorialPanel;
    public GameObject pauseButton;

    void Start()
    {
        // Khi vào game → hiện menu
        mainMenuPanel.SetActive(true);
        tutorialPanel.SetActive(false);
        pauseButton.SetActive(false);

        Time.timeScale = 0f; // pause game
    }

    public void StartGame()
    {
        mainMenuPanel.SetActive(false);
        tutorialPanel.SetActive(false);
        pauseButton.SetActive(true);


        Time.timeScale = 1f;

        GameManager.Instance.StartStage(0);
    }

    public void OpenTutorial()
    {
        mainMenuPanel.SetActive(false);
        tutorialPanel.SetActive(true);
    }

    public void BackToMenu()
    {
        tutorialPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
        pauseButton.SetActive(false);
    }
}