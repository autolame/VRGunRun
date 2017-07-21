using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPlaneManager : MonoBehaviour
{
    public EnemyPlane EnemyPlanePrefab;
    public List<EnemyPlaneSpawn> PlaneSpawnTransformList = new List<EnemyPlaneSpawn>();
    public List<Transform> PlaneTargetTransformList = new List<Transform>();

    public List<float> spawnIntervals;

    public float MinSpawnInterval = 10; // in seconds
    public float MaxSpawninterval = 30;

    public float MinMoveSpeed = 0.5f;
    public float MaxMoveSpeed = 2;

    float timeUntilSpawn;
    float spawnInterval;
    int target;

    private void Awake()
    {
        foreach (var spawn in PlaneSpawnTransformList)
        {
            spawn.SpawnInterval = Random.Range(MinSpawnInterval, MaxSpawninterval);
        }
    }

    private void Update()
    {
        target = Random.Range(1, PlaneTargetTransformList.Count);

        timeUntilSpawn += Time.deltaTime;

        foreach (var spawn in PlaneSpawnTransformList)
        {
            if (timeUntilSpawn > spawn.SpawnInterval)
            {
                timeUntilSpawn = 0;
                SpawnEmemyPlane(spawn);
            }
        }
    }

    void SpawnEmemyPlane(EnemyPlaneSpawn spawn)
    {
        EnemyPlane spawnedEnemyPlane = Instantiate(EnemyPlanePrefab);
        spawnedEnemyPlane.transform.position = spawn.transform.position;
        spawnedEnemyPlane.TargetTransform = PlaneTargetTransformList[target - 1].transform;
        spawnedEnemyPlane.MoveSpeed = Random.Range(MinMoveSpeed, MaxMoveSpeed);
        spawn.SpawnInterval = Random.Range(MinSpawnInterval, MaxSpawninterval);
    }
}
