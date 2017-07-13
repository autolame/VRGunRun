using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(CapsuleCollider))]
public class EnemyHitZone : MonoBehaviour
{
    public Enemy Enemy;
    public float HitVelocity;

    public enum HitZoneType
    {
        Head, Torso, Arm, Leg
    }
    public HitZoneType HitType = HitZoneType.Torso;

    private void Awake()
    {
        Enemy = transform.root.GetComponent<Enemy>();
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
