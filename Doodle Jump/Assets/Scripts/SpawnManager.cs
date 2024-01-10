using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject[] platformPrefabs;
    [SerializeField] private GameObject fakePlatformPrefab;
    [SerializeField] private float minY = 0.5f;
    [SerializeField] private float maxY = 2.5f;
    [SerializeField] private int numberOfPlatforms = 50;
    private GameManager _gameManager;
    
    
    public float SpawnPlatforms(float firstY,int level)
    {
        minY = 0.5f + (0.08f *  level); //set minY base on level
        if (minY > 2)
            minY = 2;
        var firstPlatformPosition = new Vector2(Random.Range(-2.0f, 2.1f),firstY);
        Instantiate(platformPrefabs[0], firstPlatformPosition, Quaternion.identity, transform);

        Vector3 spawnPosition = firstPlatformPosition;

        for (int i = 1; i < numberOfPlatforms - 1; i++)
        {
            spawnPosition.y += Random.Range(minY, maxY);
            spawnPosition.x = Random.Range(-2.0f, 2.1f);
            GameObject platformPrefab;
            if (Random.Range(0, 100) < (90 - (level * 5)))
                platformPrefab = platformPrefabs[0];
            else
                platformPrefab = platformPrefabs[1];
            var spawnedPaltform = Instantiate(platformPrefab, spawnPosition, Quaternion.identity);
            spawnedPaltform.transform.parent = this.transform;
        }

        SpawnFakePlatforms(firstY + 5, level);
        return spawnPosition.y + minY;
    }
    
    private float SpawnFakePlatforms(float firstY,int level)
    {
        float numberOfFakePlatforms = numberOfPlatforms * level / 3;
        var firstPlatformPosition = new Vector2(Random.Range(-2.0f, 2.1f),firstY);
        Instantiate(fakePlatformPrefab, firstPlatformPosition, Quaternion.identity, transform);

        Vector3 spawnPosition = firstPlatformPosition;
        
        for (int i = 1; i < numberOfFakePlatforms - 1; i++)
        {
            Debug.Log("Weak platform Created");
            spawnPosition.y += Random.Range(minY, maxY * 3 / level);
            spawnPosition.x = Random.Range(-2.0f, 2.1f);

            GameObject platformPrefab = platformPrefabs[Random.Range(0, platformPrefabs.Length)];
            Instantiate(platformPrefab, spawnPosition, Quaternion.identity, transform);
        }

        return spawnPosition.y;
    }
}