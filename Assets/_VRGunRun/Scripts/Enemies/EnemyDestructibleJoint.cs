using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDestructibleJoint : MonoBehaviour
{
    public EnemyMech HostEnemy;

    public float ZoneHealth = 1000; // this zones health
    public float HostHealthPercentage = 50; // if destroyed, host will be damaged by this percentage
    public float HostMovementSpeedPercentage = 50; // if destroyed, host will be slowed down by this percentage

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<GunAmmoBullet>())
        {
            float hitDamage = collision.gameObject.GetComponent<Rigidbody>().velocity.magnitude;
            ZoneHealth -= hitDamage;

            if (IsDestroyed)
            {
                DetachJointChildren();
                ApplyDamageToHost(hitDamage);
                Destroy(gameObject);
            }
        }
    }

    bool IsDestroyed
    {
        get
        {
            if (ZoneHealth > 0)
            { return false; }
            else { return true; }
        }
    }

    void ApplyDamageToHost(float damage)
    {
        if (HostEnemy)
        {
            HostEnemy.DamageRawHitPoint(damage);
        }
    }

    void DetachJointChildren()
    {
        foreach (var child in GetComponentsInChildren<Transform>())
        {
            child.SetParent(transform.parent);
            child.gameObject.AddComponent<Rigidbody>();
        }
    }
}
