using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private int level;
    private int lives;
    private int score;
    public static GameManager Instance { get; private set; }

    private TextMeshProUGUI livesText;
    private TextMeshProUGUI scoreText;
    private TextMeshProUGUI levelIntroText;

<<<<<<< Updated upstream
=======
<<<<<<< HEAD
    // --- Timing --- //
    private float currentLevelTime;
    private bool timerRunning;
    private readonly Dictionary<int, float> levelTimes = new Dictionary<int, float>();
    private float totalTime;

    // read-only access for GameTimer (display) and the end screen
    public float CurrentLevelTime => currentLevelTime;
    public IReadOnlyDictionary<int, float> LevelTimes => levelTimes;
    public float TotalTime => totalTime;

    private void Start()
=======
>>>>>>> Stashed changes
    private float elapsedTime;
    private bool timerRunning;

    private void Update()
<<<<<<< Updated upstream
    {
        if (timerRunning)
        {
            elapsedTime += Time.deltaTime;
=======
>>>>>>> 0707ec171b841f3a05d3ae51a693fa5069fbb106
    {
        if (timerRunning)
        {
<<<<<<< HEAD
            Destroy(gameObject);
            return;
=======
            elapsedTime += Time.deltaTime;
>>>>>>> 0707ec171b841f3a05d3ae51a693fa5069fbb106
>>>>>>> Stashed changes
        }
    }

    public void StartTimer()
    {
        elapsedTime = 0f;
        timerRunning = true;
    }

    public float GetElapsedTime() => elapsedTime;

    private void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
<<<<<<< Updated upstream
=======
<<<<<<< HEAD

    private void Update()
    {
        if (timerRunning)
        {
            currentLevelTime += Time.deltaTime;
        }
    }
=======
>>>>>>> 0707ec171b841f3a05d3ae51a693fa5069fbb106
>>>>>>> Stashed changes

    public void StartGame()
    {
        NewGame();
    }

    private void NewGame()
    {
        lives = 3;
        score = 0;
<<<<<<< Updated upstream
        StartTimer();
=======
<<<<<<< HEAD
        levelTimes.Clear();
        totalTime = 0f;
=======
        StartTimer();
>>>>>>> 0707ec171b841f3a05d3ae51a693fa5069fbb106
>>>>>>> Stashed changes
        LoadLevel(1);
    }

    private void LoadLevel(int index)
    {
        level = index;
<<<<<<< Updated upstream
        Time.timeScale = 1f; // always ensure gameplay starts unfrozen
=======
<<<<<<< HEAD

        // fresh clock every time player enters level, including retries
        currentLevelTime = 0f;
        timerRunning = true;

=======
        Time.timeScale = 1f; // always ensure gameplay starts unfrozen
>>>>>>> 0707ec171b841f3a05d3ae51a693fa5069fbb106
>>>>>>> Stashed changes
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
        // wait one frame so that the new scene's objects exist
        yield return null;

        // undo the cullingMask = 0 from LoadLevel so the level actually renders
        Camera camera = Camera.main;
        if (camera != null)
        {
            camera.cullingMask = -1; // -1 = Everything
        }

        // re-find UI references since old scene's canvas was destroyed
        GameObject livesObj = GameObject.Find("LivesText");
        if (livesObj != null) livesText = livesObj.GetComponent<TextMeshProUGUI>();
        UpdateLivesUI();

        GameObject scoreObj = GameObject.Find("ScoreText"); // new
        if (scoreObj != null) scoreText = scoreObj.GetComponent<TextMeshProUGUI>();
        UpdateScoreUI(); // new

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

        // flash settings
        int flashCount = 6;    // how many times it blinks
        float flashInterval = 0.2f;   // seconds between on/off

        for (int i = 0; i < flashCount; i++)
        {
            levelIntroText.enabled = !levelIntroText.enabled;
            yield return new WaitForSeconds(flashInterval);
        }

        levelIntroText.enabled = true;   // make sure it ends visible, not mid-flash
        yield return new WaitForSeconds(1f);   // hold fully visible for a bit

        levelIntroText.gameObject.SetActive(false);
    }

    private void UpdateLivesUI()
    {
        if (livesText != null)
        {
            livesText.text = "Lives: " + lives;
        }
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }
    }

    public void LevelComplete()
    {
        timerRunning = false;
        levelTimes[level] = currentLevelTime;
        totalTime += currentLevelTime;

        score += 1000;
        UpdateScoreUI();
        int nextLevel = level + 1;

        if (nextLevel < SceneManager.sceneCountInBuildSettings)
        {
            LoadLevel(nextLevel);
        }
        else
        {
            EndGame();
        }
    }

    // TODO: once the end-game results screen exists, add it to Build Settings and
    // load it here, until then this just logs the results
    private void EndGame()
    {
        timerRunning = false;

        Debug.Log("=== Game Complete ===");
        foreach (var kvp in levelTimes)
        {
            Debug.Log($"Level {kvp.Key}: {FormatTime(kvp.Value)}");
        }
        Debug.Log($"Total: {FormatTime(totalTime)}");
    }

    private string FormatTime(float t)
    {
        int minutes = Mathf.FloorToInt(t / 60f);
        int seconds = Mathf.FloorToInt(t % 60f);
        return $"{minutes:00}:{seconds:00}";
    }

    public void LevelFailed()
    {
        timerRunning = false;
        lives--;
        UpdateLivesUI();

        if (lives <= 0)
        {
            NewGame();
        } else
        {
            LoadLevel(level);
        }
    }
}