using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public int MaxEnemyActiveEnemiesCount = 100;
    public List<Enemy> activeEnemies = new List<Enemy>();

    public bool IsNewEnemySpawnPossible
    {
        get
        {
            return activeEnemies.Count <= MaxEnemyActiveEnemiesCount;
        }
    }
}
