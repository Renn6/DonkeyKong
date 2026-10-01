using UnityEngine;
using TMPro;

<<<<<<< Updated upstream
=======
<<<<<<< HEAD
// Pure display component: reads the running level time from GameManager,
// which is the persistent source of truth across scene loads

=======
>>>>>>> 0707ec171b841f3a05d3ae51a693fa5069fbb106
>>>>>>> Stashed changes
public class GameTimer : MonoBehaviour
{
    public TextMeshProUGUI timerText;

    private void Update()
    {
<<<<<<< Updated upstream
        if (GameManager.Instance == null) return;

=======
<<<<<<< HEAD
        if (GameManager.Instance == null || timerText == null) return;

        UpdateDisplay(GameManager.Instance.CurrentLevelTime);
    }
    void UpdateDisplay(float elapsedTime)
=======
        if (GameManager.Instance == null) return;

>>>>>>> Stashed changes
        float elapsed = GameManager.Instance.GetElapsedTime();
        UpdateDisplay(elapsed);
    }

    private void UpdateDisplay(float elapsedTime)
<<<<<<< Updated upstream
=======
>>>>>>> 0707ec171b841f3a05d3ae51a693fa5069fbb106
>>>>>>> Stashed changes
    {
        int minutes = Mathf.FloorToInt(elapsedTime / 60f);
        int seconds = Mathf.FloorToInt(elapsedTime % 60f);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
<<<<<<< Updated upstream
=======
<<<<<<< HEAD
>>>>>>> Stashed changes
}
=======
}
>>>>>>> 0707ec171b841f3a05d3ae51a693fa5069fbb106
