using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] float spawnInterval = 2.0f;
    [SerializeField] float rangeX = 5f;
    [SerializeField] float rangeY = 5f;
    float timer = 0f;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnEnemy(); // メソッドを呼び出す
            timer = 0f;
        }
    }

    // 汎用性を高めるために独立させた生成メソッド
    void SpawnEnemy()
    {
        float randomX = Random.Range(-rangeX, rangeX);
        float randomY = Random.Range(-rangeY, rangeY);
        Vector3 randomPos = new Vector3(randomX, randomY, 0);

        Instantiate(enemyPrefab, randomPos, Quaternion.identity);
    }
}
