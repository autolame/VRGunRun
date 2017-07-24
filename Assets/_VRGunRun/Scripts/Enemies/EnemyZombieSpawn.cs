using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyZombieSpawn : MonoBehaviour
{
    public EnemyZombie Prefab;
    public EnemyGoal Goal;
    public float SpawnFrequency = 1;  // spawn per second
    private float spawnTimer;

    private void Update()
    {
        spawnTimer -= Time.deltaTime;

        if (spawnTimer < 0)
        {
            spawnTimer = SpawnFrequency;
            SpawnEnemy(Prefab);
        }
    }

    public void SpawnEnemy(EnemyZombie enemy)
    {
        var spawnedEnemy = Instantiate(Prefab);
        spawnedEnemy.gameObject.SetActive(false);
        spawnedEnemy.transform.position = this.transform.position;
        spawnedEnemy.gameObject.SetActive(true);
        spawnedEnemy.Goal = Goal;
        spawnedEnemy.MoveTowardsGoal();
    }
}
