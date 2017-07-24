using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGoal : MonoBehaviour
{

    public int HitPoints = 30;

    public Vector3 Position
    {
        get { return transform.position; }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.root.gameObject.GetComponent<EnemyZombie>())
        {
            HitPoints--;
            if (HitPoints < 1)
            {
                GetComponent<Renderer>().material.color = Color.red;
            }
            Destroy(collision.transform.root.gameObject);
        }
    }
}
