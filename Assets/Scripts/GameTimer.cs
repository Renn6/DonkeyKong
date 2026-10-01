using UnityEngine;
using TMPro;

// Pure display component: reads the running level time from GameManager,
// which is the persistent source of truth across scene loads
public class GameTimer : MonoBehaviour
{
    public TextMeshProUGUI timerText;

    private void Update()
    {
        if (GameManager.Instance == null || timerText == null) return;

        UpdateDisplay(GameManager.Instance.CurrentLevelTime);
    }

    private void UpdateDisplay(float elapsedTime)
    {
        int minutes = Mathf.FloorToInt(elapsedTime / 60f);
        int seconds = Mathf.FloorToInt(elapsedTime % 60f);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
