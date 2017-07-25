using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPlaneSpawn : MonoBehaviour
{

    public float SpawnInterval;
    public float TimeUntilSpawn;

    private void Update()
    {
        TimeUntilSpawn += Time.deltaTime;
    }
}
