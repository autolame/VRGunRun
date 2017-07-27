using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyArmorPlate : MonoBehaviour
{
    public float HitPoints = 500f;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<GunAmmoBullet>())
        {
            float hitDamage = collision.gameObject.GetComponent<Rigidbody>().velocity.magnitude;
            HitPoints -= hitDamage;

            if (IsDestroyed)
            {
                var rb = gameObject.GetComponent<Rigidbody>();
                if (!rb)
                {
                    rb = gameObject.AddComponent<Rigidbody>();
                }
                rb.velocity = Vector3.up;
                gameObject.transform.parent = null;
                Destroy(gameObject, 10f);
            }
        }
    }

    bool IsDestroyed
    {
        get
        {
            if (HitPoints > 0)
            { return false; }
            else { return true; }
        }
    }
}
