using UnityEngine;

public class ExtraLife : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Only react to Mario
        if (other.GetComponent<Player>() == null) return;

        if (GameManager.Instance != null)
        {
            GameManager.Instance.AddLife();
        }

        Destroy(gameObject);
    }
}