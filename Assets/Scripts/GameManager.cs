using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private int level;
    private int lives;
    private int score;

    // Princess rescue tracking
    private int princessesRescued;
    private int princessesRequired = 1;
    private bool levelEnding;

    public static GameManager Instance { get; private set; }

    private TextMeshProUGUI livesText;
    private TextMeshProUGUI scoreText;
    private TextMeshProUGUI levelIntroText;
    private TextMeshProUGUI dieScreenText;
    private GameObject dieScreenPanel;

    private float elapsedTime;
    private bool timerRunning;

    // Results screen tracking (index 1-3 = Level 1-3, index 0 unused)
    private const int FinalLevel = 3;
    private const string ResultsSceneName = "ResultsScreen";
    private readonly float[] levelTimes = new float[FinalLevel + 1];
    private float levelStartTime;

    private void Update()
    {
        if (timerRunning)
        {
            elapsedTime += Time.deltaTime;
        }
    }

    public void StartTimer()
    {
        elapsedTime = 0f;
        timerRunning = true;
    }

    public float GetElapsedTime() => elapsedTime;
    public float GetLevelTime(int levelNumber) => levelTimes[levelNumber];
    public float GetTotalTime() => elapsedTime;

    private void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void StartGame()
    {
        NewGame();
    }

    private void NewGame()
    {
        lives = 3;
        score = 0;
        System.Array.Clear(levelTimes, 0, levelTimes.Length);
        levelStartTime = 0f;
        StartTimer();
        LoadLevel(1);
    }

    private void LoadLevel(int index)
    {
        level = index;

        // Reset rescue progress every time a level re-loads
        princessesRescued = 0;
        princessesRequired = 1;
        levelEnding = false;

        // Always ensure gameplay starts unfrozen
        Time.timeScale = 1f;
        SceneManager.LoadScene(level);
        StartCoroutine(SetupSceneAfterLoad());

        Camera camera = Camera.main;
        if (camera != null)
        {
            camera.cullingMask = 0;
        }
    }

    private IEnumerator SetupSceneAfterLoad()
    {
        yield return null;

        // Count how many princesses this level has
        princessesRequired = Mathf.Max(1, GameObject.FindGameObjectsWithTag("Objective").Length);
        princessesRescued = 0;
        Debug.Log($"Level {level} loaded: {princessesRequired} princess(es) to rescue");

        GameObject livesObj = GameObject.Find("LivesText");
        if (livesObj != null) livesText = livesObj.GetComponent<TextMeshProUGUI>();
        UpdateLivesUI();

        // Princess counter (still uses the object named "ScoreText")
        GameObject scoreObj = GameObject.Find("ScoreText");
        if (scoreObj != null) scoreText = scoreObj.GetComponent<TextMeshProUGUI>();
        else Debug.LogWarning("No object named 'ScoreText' found in this scene");
        UpdateScoreUI();

        GameObject dieObj = GameObject.Find("DieScreenText");
        if (dieObj != null) dieScreenText = dieObj.GetComponent<TextMeshProUGUI>();

        GameObject diePanelObj = GameObject.Find("DieScreenPanel");
        if (diePanelObj != null)
        {
            dieScreenPanel = diePanelObj;
            dieScreenPanel.SetActive(false);
        }

        GameObject introObj = GameObject.Find("LevelIntroText");
        if (introObj != null)
        {
            levelIntroText = introObj.GetComponent<TextMeshProUGUI>();
            StartCoroutine(ShowLevelIntro());
        }
    }

    private IEnumerator ShowLevelIntro()
    {
        if (levelIntroText == null) yield break;

        levelIntroText.text = "Level " + level;
        levelIntroText.gameObject.SetActive(true);

        int flashCount = 6;
        float flashInterval = 0.2f;

        for (int i = 0; i < flashCount; i++)
        {
            if (levelIntroText == null) yield break;
            levelIntroText.enabled = !levelIntroText.enabled;
            yield return new WaitForSeconds(flashInterval);
        }

        if (levelIntroText == null) yield break;
        levelIntroText.enabled = true;
        yield return new WaitForSeconds(1f);

        if (levelIntroText == null) yield break;
        levelIntroText.gameObject.SetActive(false);
    }

    private void UpdateLivesUI()
    {
        if (livesText != null)
        {
            livesText.text = "Lives: " + lives;
        }
    }

    // Shows princesses rescued, e.g. "Princesses: 2/3"
    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Princesses: " + princessesRescued + "/" + princessesRequired;
        }
    }

    // Called by the Player each time a princess is rescued.
    // The Player deactivates the princess BEFORE calling this, so any active
    // "Objective" still in the scene is a princess who hasn't been rescued yet.
    public void PrincessRescued()
    {
        if (levelEnding) return;

        princessesRescued++;
        score += 500;
        UpdateScoreUI();

        int remaining = GameObject.FindGameObjectsWithTag("Objective").Length;
        Debug.Log($"Princess rescued ({princessesRescued}/{princessesRequired}), {remaining} remaining");

        if (remaining <= 0)
        {
            LevelComplete();
        }
    }

    public void LevelComplete()
    {
        if (levelEnding) return;
        levelEnding = true;

        score += 1000;
        UpdateScoreUI();

        // Record how long this level took
        levelTimes[level] = elapsedTime - levelStartTime;
        levelStartTime = elapsedTime;

        if (level >= FinalLevel)
        {
            // Game finished, stop the clock and show the results screen
            timerRunning = false;
            Time.timeScale = 1f;
            SceneManager.LoadScene(ResultsSceneName);
        }
        else
        {
            LoadLevel(level + 1);
        }
    }

    public void LevelFailed()
    {
        lives--;
        UpdateLivesUI();
        StartCoroutine(ShowDieScreenThenContinue());
    }

    // Called by the ExtraLife pickup
    public void AddLife()
    {
        lives++;
        UpdateLivesUI();
    }

    private IEnumerator ShowDieScreenThenContinue()
    {
        if (dieScreenText != null)
        {
            if (lives > 0)
            {
                dieScreenText.text = "You Died!\n" + lives + " Lives Remaining\nPress Enter to Respawn";
            }
            else
            {
                dieScreenText.text = "Game Over\nPress Enter to Restart";
            }

            dieScreenPanel.SetActive(true);
            Time.timeScale = 0f;

            // Brief pause so the player doesn't skip it by accident
            yield return new WaitForSecondsRealtime(0.3f);

            while (!Input.GetKeyDown(KeyCode.Return) && !Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                yield return null;
            }

            dieScreenPanel.SetActive(false);
        }

        Time.timeScale = 1f;

        if (lives <= 0)
        {
            NewGame();
        }
        else
        {
            LoadLevel(level);
        }
    }
}