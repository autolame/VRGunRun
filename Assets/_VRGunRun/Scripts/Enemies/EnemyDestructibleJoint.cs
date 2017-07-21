using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDestructibleJoint : MonoBehaviour
{
    public Enemy HostEnemy;

    public float ZoneHealth = 1000; // this zones health
    public float HostHealthPercentage = 50; // if destroyed, host will be damaged by this percentage
    public float HostMovementSpeedPercentage = 50; // if destroyed, host will be slowed down by this percentage

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<GunAmmoBullet>())
        {
            float hitDamage = collision.gameObject.GetComponent<Rigidbody>().velocity.magnitude;
            ZoneHealth -= hitDamage;
        }

        if (IsDestroyed)
        {
            DetachChildren();
            ApplyDamageToHost();
            Destroy(gameObject);
        }
    }

    bool IsDestroyed
    {
        get
        {
            if (ZoneHealth <= 0)
            { return true; }
            else
            { return false; }
        }
    }

    void ApplyDamageToHost()
    {
        if (HostEnemy)
        {
            HostEnemy.HitPointPercentage -= HostHealthPercentage;
            HostEnemy.MovementSpeedPercentage -= HostMovementSpeedPercentage;
        }
    }
    void DetachChildren()
    {
        foreach (var child in GetComponentsInChildren<Transform>())
        {
            child.SetParent(transform.parent);
            child.gameObject.AddComponent<Rigidbody>();
        }
    }
}
