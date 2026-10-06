using System.Collections;
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
    private TextMeshProUGUI dieScreenText;
    private GameObject dieScreenPanel;

    private float elapsedTime;
    private bool timerRunning;

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
        StartTimer();
        LoadLevel(1);
    }

    private void LoadLevel(int index)
    {
        level = index;
        Time.timeScale = 1f; // always ensure gameplay starts unfrozen
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

        GameObject livesObj = GameObject.Find("LivesText");
        if (livesObj != null) livesText = livesObj.GetComponent<TextMeshProUGUI>();
        UpdateLivesUI();

        GameObject dieObj = GameObject.Find("DieScreenText");
        if (dieObj != null) dieScreenText = dieObj.GetComponent<TextMeshProUGUI>();

        GameObject diePanelObj = GameObject.Find("DieScreenPanel"); // new
        if (diePanelObj != null)
        {
            dieScreenPanel = diePanelObj;
            dieScreenPanel.SetActive(false); // disable the whole panel here, in code
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

        // flash settings
        int flashCount = 6;        // how many times it blinks
        float flashInterval = 0.2f; // seconds between on/off

        for (int i = 0; i < flashCount; i++)
        {
            levelIntroText.enabled = !levelIntroText.enabled;
            yield return new WaitForSeconds(flashInterval);
        }

        levelIntroText.enabled = true; // make sure it ends visible, not mid-flash
        yield return new WaitForSeconds(1f); // hold fully visible for a bit

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
        score += 1000;
        UpdateScoreUI();
        int nextLevel = level + 1;

        if (nextLevel < SceneManager.sceneCountInBuildSettings)
        {
            LoadLevel(nextLevel);
        }
        else
        {
            LoadLevel(1);
        }
    }

    public void LevelFailed()
    {
        lives--;
        UpdateLivesUI();
        StartCoroutine(ShowDieScreenThenContinue());
    }


    private IEnumerator ShowDieScreenThenContinue()
    {
        if (dieScreenText != null)
        {
            if (lives > 0)
            {
                dieScreenText.text = "You Died\n" + lives + " Lives Remaining\nPress Enter to Respawn";
            }
            else
            {
                dieScreenText.text = "Game Over\nPress Enter to Restart";
            }

            dieScreenPanel.SetActive(true);
            Time.timeScale = 0f;

            // brief pause so the player doesn't accidentally skip it
            // by still holding a key down from the moment they died
            yield return new WaitForSecondsRealtime(0.3f);

            // wait here until the player actually presses Enter
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
