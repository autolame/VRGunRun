using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMech : Enemy
{
    private void Update()
    {
        if (IsDestroyed)
        {
            foreach (var child in GetComponentsInChildren<GameObject>())
            {
                Destroy(gameObject, 10);
                child.gameObject.AddComponent<Rigidbody>();
                child.transform.SetParent(null);
                Destroy(child.gameObject, 10);
            }
        }
    }
}
