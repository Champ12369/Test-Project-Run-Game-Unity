using UnityEngine;

public class ObstacleGenerator : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public GameObject[] obstaclePrefabs; // Array of obstacle prefabs
    public GameObject[] coinPrefabs; // Array of coin prefabs
    public float spawnInterval = 10.0f; // Distance between spawn points
    private float nextSpawnZ = 0.0f;

    void Update()
    {
        // Move the player forward along its local forward axis
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);

        // Check if it's time to spawn obstacles or coins
        if (transform.position.z >= nextSpawnZ)
        {
            SpawnObstacleOrCoin();
            // Set the next spawn position
            nextSpawnZ += spawnInterval;
        }
    }

    void SpawnObstacleOrCoin()
    {
        // Randomly choose an obstacle prefab from the array
        GameObject obstaclePrefab = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)];
        // Randomly choose a coin prefab from the array
        GameObject coinPrefab = coinPrefabs[Random.Range(0, coinPrefabs.Length)];

        // Randomly choose an x position from -3, 0, or 3
        float randomX = Random.Range(-3f, 4f);
        randomX = Mathf.Round(randomX); // Round to -3, 0, or 3

        // Set the spawn position
        Vector3 spawnPosition = new Vector3(randomX, 0.5f, nextSpawnZ); // Adjust the y as needed

        // Instantiate the selected obstacle prefab at the spawn position
        Instantiate(obstaclePrefab, spawnPosition, Quaternion.identity);

        // Randomly choose whether to spawn a coin
        bool spawnCoin = Random.Range(0, 2) == 0;
        if (spawnCoin)
        {
            // Instantiate the selected coin prefab at the spawn position
            Instantiate(coinPrefab, spawnPosition, Quaternion.identity);
        }
    }
}
