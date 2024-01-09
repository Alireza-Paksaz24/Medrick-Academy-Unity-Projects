using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject[] platformPrefabs;
    [SerializeField] private float minY = 0.3f;
    [SerializeField] private float maxY = 2.5f;
    [SerializeField] private int numberOfPlatforms = 20;
    

    void Start()
    {
        SpawnPlatforms(-4,numberOfPlatforms * 0.7f);
    }

    public void SpawnPlatforms(float firstY, float lastY)
    {
        var firstPlatformPosition = new Vector2(Random.Range(-2.0f, 2.1f),firstY);
        var lastPlatformPosition = new Vector2(Random.Range(-2.0f, 2.1f),lastY);
        Instantiate(platformPrefabs[0], firstPlatformPosition, Quaternion.identity, transform);

        Vector3 spawnPosition = firstPlatformPosition;

        for (int i = 1; i < numberOfPlatforms - 1; i++)
        {
            spawnPosition.y += Random.Range(minY, maxY);
            spawnPosition.x = Random.Range(-2.0f, 2.1f);

            GameObject platformPrefab = platformPrefabs[Random.Range(0, platformPrefabs.Length)];
            Instantiate(platformPrefab, spawnPosition, Quaternion.identity, transform);
        }

        
        Instantiate(platformPrefabs[0], lastPlatformPosition, Quaternion.identity, transform);
    }
    
    
}