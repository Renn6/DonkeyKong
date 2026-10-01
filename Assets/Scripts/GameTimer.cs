using UnityEngine;
using TMPro;

public class GameTimer : MonoBehaviour
{
    public TextMeshProUGUI timerText;

    private void Update()
    {
        if (GameManager.Instance == null) return;

        float elapsed = GameManager.Instance.GetElapsedTime();
        UpdateDisplay(elapsed);
    }

    private void UpdateDisplay(float elapsedTime)
    {
        int minutes = Mathf.FloorToInt(elapsedTime / 60f);
        int seconds = Mathf.FloorToInt(elapsedTime % 60f);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
