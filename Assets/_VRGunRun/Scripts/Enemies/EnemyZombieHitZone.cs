using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(CapsuleCollider))]
public class EnemyZombieHitZone : MonoBehaviour
{
    public EnemyZombie Enemy;
    public float HitVelocity;

    public enum HitZoneType
    {
        Head, Torso, Arm, Leg, Armor, Core
    }
    public HitZoneType HitType = HitZoneType.Torso;

    private void Awake()
    {
        Enemy = transform.root.GetComponent<EnemyZombie>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<GunAmmoBullet>())
        {
            HitVelocity = collision.gameObject.GetComponent<Rigidbody>().velocity.magnitude;
            Enemy.GotHitWith(HitVelocity, HitType);
        }
    }
}
