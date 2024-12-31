using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private int spawnCount = 10;
    [SerializeField] private Vector3 minBounds = new(0, 0, 0);
    [SerializeField] private Vector3 maxBounds = new(10, 10, 10);

    private void Start()
    {
        SpawnPrefabs();
    }

    private void SpawnPrefabs()
    {
        if (prefab == null)
        {
            Debug.LogError("Prefab is not assigned!");
            return;
        }

        for (int i = 0; i < spawnCount; i++)
        {
            Vector3 randomPosition = new (
                Random.Range(minBounds.x, maxBounds.x),
                Random.Range(minBounds.y, maxBounds.y),
                Random.Range(minBounds.z, maxBounds.z)
            );

            Instantiate(prefab, randomPosition, Random.rotation, transform);
        }
    }
}
