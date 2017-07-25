using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class EnemyZombie : Enemy
{
    public EnemyZombieRagdoll RagdollController;

    public float HeadDamagePercent = 100f;
    public float TorsoDamagePercent = 20f;
    public float LegDamagePercent = 10f;
    public float ArmDamagePercent = 5f;

    public int AnimState = 0;
    private Animator animator;
    private void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();
    }
    private void Start()
    {
        AnimState = Random.Range(1, 4);
        animator.SetInteger("AnimState", AnimState);
    }
    private void Update()
    {
        if (IsDestroyed)
        {
            SpawnRagdoll();
            Destroy(gameObject);
        }
    }

    public void GotHitWith(float velocity, EnemyZombieHitZone.HitZoneType hitType)
    {

        // TODO make damage more uniformly
        switch (hitType)
        {
            case EnemyZombieHitZone.HitZoneType.Head:
                //DamageRawHitPoint(velocity / 100 * HeadDamagePercent);
                DamagePercentageHitPoint(HeadDamagePercent);
                break;
            case EnemyZombieHitZone.HitZoneType.Torso:
                DamageRawHitPoint(velocity / 100 * TorsoDamagePercent);
                //DamagePercentageHitPoint(TorsoDamagePercent);
                break;
            case EnemyZombieHitZone.HitZoneType.Leg:
                DamageRawHitPoint(velocity / 100 * LegDamagePercent);
                //DamagePercentageHitPoint(LegDamagePercent);
                break;
            case EnemyZombieHitZone.HitZoneType.Arm:
                DamageRawHitPoint(velocity / 100 * ArmDamagePercent);
                //DamagePercentageHitPoint(ArmDamagePercent);
                break;
        }
    }

    public void SpawnRagdoll()
    {
        var ragdoll = Instantiate(RagdollController);
        ragdoll.transform.position = transform.position;
        ragdoll.transform.rotation = transform.rotation;
        ragdoll.Enemy = this;
        ragdoll.GetRagdollPose();
        ragdoll.PoseRagdoll();
        ragdoll.gameObject.SetActive(true);
        Destroy(ragdoll.gameObject, 30f);
    }
}
