using UnityEngine;

public class StartSpawner : MonoBehaviour
{
    public GameObject prefab;

    private void Awake()
    {
        Instantiate(prefab, transform.position, Quaternion.identity);
        Invoke(nameof(Barrel), Random.Range(0,1));
    }

}
