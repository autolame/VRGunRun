using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMech : Enemy
{
    private void Awake()
    {
        base.Awake();
    }

    private void Update()
    {
        if (IsDestroyed)
        {
            foreach (var child in GetComponentsInChildren<Transform>())
            {
                if (!child.gameObject.GetComponent<Rigidbody>())
                {
                    child.gameObject.AddComponent<Rigidbody>();
                }
                Destroy(child.gameObject, 10);
                child.SetParent(null);
            }
            Destroy(gameObject, 30f);
        }
    }
}
