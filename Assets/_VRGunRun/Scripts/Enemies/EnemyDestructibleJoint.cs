using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDestructibleJoint : MonoBehaviour
{
    public Enemy HostEnemy;

    public float ZoneHealth = 1000; // this zones health
    public float HostHealthPercentage = 50; // if destroyed, host will be damaged by this percentage
    public float HostMovementSpeedPercentage = 50; // if destroyed, host will be slowed down by this percentage
    bool IsDestroyed
    {
        get
        {
            if (ZoneHealth > 0)
            { return false; }
            else { return true; }
        }
    }

    private void Awake()
    {
        HostEnemy = transform.root.GetComponent<Enemy>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<GunAmmoBullet>())
        {
            float hitDamage = collision.gameObject.GetComponent<Rigidbody>().velocity.magnitude;
            ZoneHealth -= hitDamage;

            if (IsDestroyed)
            {
                DetachJointChildren();
                ApplyDamageToHost(HostHealthPercentage);
                Destroy(gameObject);
            }
        }
    }
    void ApplyDamageToHost(float damage)
    {
        if (HostEnemy)
        {
            HostEnemy.DamagePercentageHitPoint(damage);
        }
    }
    void DetachJointChildren()
    {
        foreach (var child in GetComponentsInChildren<Transform>())
        {
            child.SetParent(transform.parent);
            if (!child.GetComponent<Rigidbody>())
                child.gameObject.AddComponent<Rigidbody>();
            Destroy(child.gameObject, 1f);
        }
    }
}
