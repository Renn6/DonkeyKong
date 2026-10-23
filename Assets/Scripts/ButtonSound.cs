using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonSound : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            if (AudioManager.Instance != null) AudioManager.Instance.PlayButton();
        });
    }
}