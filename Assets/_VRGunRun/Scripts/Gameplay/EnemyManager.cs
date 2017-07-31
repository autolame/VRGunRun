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
            return activeEnemies.Count < MaxEnemyActiveEnemiesCount;
        }
    }

    public void CleanUpDestroyedEnemies()
    {
        for (int i = activeEnemies.Count - 1; i >= 0; i--)
        {
            if (activeEnemies[i] == null)
            {
                activeEnemies.Remove(activeEnemies[i]);
            }
        }
    }
}
