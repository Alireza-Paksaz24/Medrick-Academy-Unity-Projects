using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject[] platformPrefabs;
    [SerializeField] private GameObject fakePlatformPrefab;
    [SerializeField] private float minY = 0.3f;
    [SerializeField] private float maxY = 2.5f;
    [SerializeField] private int numberOfPlatforms = 20;
    private GameManager _gameManager;
    
    
    public float SpawnPlatforms(float firstY,int level)
    {
        minY = (float) (0.3 + (0.1 * level)); //set minY base on level
        var firstPlatformPosition = new Vector2(Random.Range(-2.0f, 2.1f),firstY);
        Instantiate(platformPrefabs[0], firstPlatformPosition, Quaternion.identity, transform);

        Vector3 spawnPosition = firstPlatformPosition;

        for (int i = 1; i < numberOfPlatforms - 1; i++)
        {
            spawnPosition.y += Random.Range(minY, maxY);
            spawnPosition.x = Random.Range(-2.0f, 2.1f);

            GameObject platformPrefab = platformPrefabs[Random.Range(0, platformPrefabs.Length)];
            Instantiate(platformPrefab, spawnPosition, Quaternion.identity, transform);
        }

        SpawnFakePlatforms(firstY + 5, level);
        return spawnPosition.y;
    }
    
    private float SpawnFakePlatforms(float firstY,int level)
    {
        var firstPlatformPosition = new Vector2(Random.Range(-2.0f, 2.1f),firstY);
        Instantiate(fakePlatformPrefab, firstPlatformPosition, Quaternion.identity, transform);

        Vector3 spawnPosition = firstPlatformPosition;

        for (int i = 1; i < (numberOfPlatforms * level* 0.2) - 1; i++)
        {
            spawnPosition.y += Random.Range(minY, maxY);
            spawnPosition.x = Random.Range(-2.0f, 2.1f);

            GameObject platformPrefab = platformPrefabs[Random.Range(0, platformPrefabs.Length)];
            Instantiate(platformPrefab, spawnPosition, Quaternion.identity, transform);
        }

        return spawnPosition.y;
    }
}