using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public float StartHitPoint = 2500f;
    public List<Enemy> EnemiesInRange = new List<Enemy>();

    public Enemy nearestEnemy;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Enemy>())
        {
            var enemy = collision.gameObject.GetComponent<Enemy>();
            EnemiesInRange.Add(enemy);
        }

        foreach (var enemy in EnemiesInRange)
        {
            if (enemy == null)
            {
                EnemiesInRange.Remove(enemy);
            }
        }
    }

    private void Update()
    {
        foreach (var enemy in EnemiesInRange)
        {
            if (enemy != null)
            {
                // TODO implement
            }
        }
    }
}
