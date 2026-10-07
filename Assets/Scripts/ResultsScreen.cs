using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultsScreen : MonoBehaviour
{
    public TextMeshProUGUI level1TimeText;
    public TextMeshProUGUI level2TimeText;
    public TextMeshProUGUI level3TimeText;
    public TextMeshProUGUI totalTimeText;

    private void Start()
    {
        Time.timeScale = 1f;

        GameManager gm = GameManager.Instance;
        if (gm == null) return; // e.g. testing this scene on its own

        level1TimeText.text = "Level 1:  " + FormatTime(gm.GetLevelTime(1));
        level2TimeText.text = "Level 2:  " + FormatTime(gm.GetLevelTime(2));
        level3TimeText.text = "Level 3:  " + FormatTime(gm.GetLevelTime(3));
        totalTimeText.text  = "Total Time:  " + FormatTime(gm.GetTotalTime());
    }

    // Hook these up to buttons' OnClick
    public void PlayAgain()
    {
        GameManager.Instance.StartGame();
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    private static string FormatTime(float t)
    {
        int minutes = Mathf.FloorToInt(t / 60f);
        int seconds = Mathf.FloorToInt(t % 60f);
        int hundredths = Mathf.FloorToInt((t * 100f) % 100f);
        return string.Format("{0:00}:{1:00}.{2:00}", minutes, seconds, hundredths);
    }
}
