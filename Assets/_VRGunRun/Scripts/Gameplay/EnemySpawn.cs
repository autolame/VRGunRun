using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    public Enemy EnemyPrefab;
    public float SpawnFrequency = 1;  // spawn per second
    private float spawnTimer;

    private void Update()
    {
        spawnTimer -= Time.deltaTime;

        if (spawnTimer < 0)
        {
            spawnTimer = SpawnFrequency;
            SpawnEnemy(EnemyPrefab);
        }
    }

    public void SpawnEnemy(Enemy enemy)
    {
        var spawnedEnemy = Instantiate(EnemyPrefab);
        spawnedEnemy.gameObject.SetActive(false);
        spawnedEnemy.transform.position = this.transform.position;
        spawnedEnemy.gameObject.SetActive(true);
        spawnedEnemy.MoveTowardsGoal();
    }
}
