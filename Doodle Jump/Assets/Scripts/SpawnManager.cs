using TMPro;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject _platformContainer;
    [SerializeField] private GameObject _enemyContainer;
    [SerializeField] private GameObject[] platformPrefabs;
    [SerializeField] private GameObject fakePlatformPrefab;
    [SerializeField] private GameObject[] enemiesPrefabs;
    [SerializeField] private float minY = 0.5f;
    [SerializeField] private float maxY = 1.8f;
    [SerializeField] private int numberOfPlatforms = 40;
    private GameManager _gameManager;
    
    
    public float SpawnPlatforms(float firstY,int level)
    {
        minY = 0.5f + (0.08f *  level); //set minY base on level
        if (minY > 2)
            minY = 2;
        maxY = 1.8f + (0.08f *  level); //set minx base on level
        if (maxY > 2)
            maxY = 2;
        var firstPlatformPosition = new Vector2(Random.Range(-2.0f, 2.1f),firstY);
        var firstPlatformInstantiate = Instantiate(platformPrefabs[0], firstPlatformPosition, Quaternion.identity);
        firstPlatformInstantiate.transform.parent = _platformContainer.transform;

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
            if (Random.Range(0, 100) > (90 - (level * 3)))
            {
                var spawnedFakePaltform = Instantiate(fakePlatformPrefab, spawnPosition + new Vector3(Random.Range(-2.0f, 2.1f),0.5f), Quaternion.identity);
                spawnedFakePaltform.transform.parent = _platformContainer.transform;
            }
            var spawnedPaltform = Instantiate(platformPrefab, spawnPosition, Quaternion.identity);
            spawnedPaltform.transform.parent = _platformContainer.transform;
        }

        var numberOfEnemies = 3 * (level - 1);
        if (numberOfEnemies > 15)
            numberOfEnemies = 15;
        SpawnEnemy(firstY, numberOfPlatforms, numberOfEnemies);
        return spawnPosition.y + minY;
    }

    private void SpawnEnemy(float firstY, int numberOfPlats, int numberOfEnemies)
    {
        if (numberOfEnemies == 0)
            return;
        var mX = maxY * numberOfPlatforms / numberOfEnemies;
        Vector3 spawnPosition = new Vector2(Random.Range(-2.0f, 2.1f),firstY);;
        for (int i = 1; i < numberOfEnemies; i++)
        {
            spawnPosition.y += Random.Range(3, mX);
            spawnPosition.x = Random.Range(-2.0f, 2.1f);
            GameObject enemyPrefab;
            var enemyType = Random.Range(0, 100);
            if (enemyType < 80)
                enemyPrefab = enemiesPrefabs[0];
            else if (80 <= enemyType && enemyType > 95)
                enemyPrefab = enemiesPrefabs[1];
            else
                enemyPrefab = enemiesPrefabs[2];
            var spawnedEnemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
            spawnedEnemy.transform.parent = _enemyContainer.transform;
        }
    }
}