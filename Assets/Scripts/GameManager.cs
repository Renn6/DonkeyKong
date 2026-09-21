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
    private TextMeshProUGUI levelIntroText;

    private void Start()
    {
        if (Instance != null)
        {
          Destroy(gameObject);
         return;
        }
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
        LoadLevel(1);
    }

    private void LoadLevel(int index)
    {
        level = index;
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
        // wait one frame so the new scene's objects exist
        yield return null;

        // re-find UI references since old scene's Canvas was destroyed
        GameObject livesObj = GameObject.Find("LivesText");
        if (livesObj != null) livesText = livesObj.GetComponent<TextMeshProUGUI>();
        UpdateLivesUI();

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

    public void LevelComplete()
    {
        score += 1000;
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

        if (lives <= 0)
        {
            NewGame();
        } else
        {
            LoadLevel(level);
        }
    }
}
