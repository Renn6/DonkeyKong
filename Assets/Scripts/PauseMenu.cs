using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pausePanel;
    public GameObject confirmQuitPanel;
    private bool isPaused = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Escape detected, isPaused = " + isPaused);
            if (isPaused) Resume();
            else Pause();
        }
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
        confirmQuitPanel.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void RequestQuitToMainMenu()
    {
        pausePanel.SetActive(false);
        confirmQuitPanel.SetActive(true);
    }

    public void CancelQuitToMainMenu()
    {
        confirmQuitPanel.SetActive(false);
        pausePanel.SetActive(true);
    }

    public void ConfirmQuitToMainMenu()
    {
        Time.timeScale = 1f;
        isPaused = false;
        SceneManager.LoadScene("MainMenu");
    }
}
