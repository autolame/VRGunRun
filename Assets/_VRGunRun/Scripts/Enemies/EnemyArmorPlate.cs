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
                if (!gameObject.GetComponent<Rigidbody>())
                {
                    gameObject.AddComponent<Rigidbody>();
                }
                gameObject.AddComponent<Rigidbody>().velocity = Vector3.up;
                gameObject.transform.parent = null;
                Destroy(gameObject, 10f);
                Destroy(this);
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
