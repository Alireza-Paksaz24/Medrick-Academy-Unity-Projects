using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    public GameObject[] platformPrefabs;
    public float minY = 2.0f;
    public float maxY = 4.0f;
    public int numberOfPlatforms = 20;

    public Vector3 firstPlatformPosition;
    public Vector3 lastPlatformPosition;

    void Start()
    {
        SpawnPlatforms();
    }

    void SpawnPlatforms()
    {
        
        Instantiate(platformPrefabs[0], firstPlatformPosition, Quaternion.identity, transform);

        Vector3 spawnPosition = firstPlatformPosition;

        for (int i = 1; i < numberOfPlatforms - 1; i++)
        {
            spawnPosition.y += Random.Range(minY, maxY);
            spawnPosition.x = Random.Range(-5.0f, 5.0f);

            GameObject platformPrefab = platformPrefabs[Random.Range(0, platformPrefabs.Length)];
            Instantiate(platformPrefab, spawnPosition, Quaternion.identity, transform);
        }

        
        Instantiate(platformPrefabs[0], lastPlatformPosition, Quaternion.identity, transform);
    }
}