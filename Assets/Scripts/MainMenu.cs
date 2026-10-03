using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        GameManager.Instance.StartGame();
    }

    public void QuitGame()
    {
        Debug.Log("Quit game"); // shows in Console when testing in editor
        Application.Quit();
    }
}
