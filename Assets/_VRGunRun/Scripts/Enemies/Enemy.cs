using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class Enemy : MonoBehaviour
{
    public float HitPointPercentage = 100f;
    public float MovementSpeedPercentage = 100f;


    public float HitPointRaw = 100f; // default hp for this enemy
    public float MovementSpeedRaw = 1f;  // default movement speed for this enemy

    public EnemyRagdoll RagdollController;

    public float HeadDamagePercent = 100f;
    public float TorsoDamagePercent = 20f;
    public float LegDamagePercent = 10f;
    public float ArmDamagePercent = 5f;

    public int AnimState = 0;

    public EnemyGoal Goal;
    private NavMeshAgent agent;
    private Animator Animator;

    private void Awake()
    {
        Goal = FindObjectOfType<EnemyGoal>();
        agent = GetComponent<NavMeshAgent>();
        Animator = GetComponent<Animator>();
        MoveTowardsGoal();
    }

    private void Start()
    {
        AnimState = Random.Range(1, 4);
        Animator.SetInteger("AnimState", AnimState);
    }
    private void Update()
    {
        if (IsDead)
        {
            SpawnRagdoll();
            Destroy(gameObject);
        }
    }

    public void MoveTowardsGoal()
    {
        agent.destination = Goal.Position;
    }

    public bool IsDead
    {
        get
        {
            if (HitPointRaw > 0)
                return false;
            else
                return true;
        }
    }

    public void GotHitWith(float velocity, EnemyHitZone.HitZoneType hitType)
    {
        switch (hitType)
        {
            case EnemyHitZone.HitZoneType.Head:
                HitPointRaw -= velocity / 100 + HeadDamagePercent;
                break;
            case EnemyHitZone.HitZoneType.Torso:
                HitPointRaw -= velocity / 100 + TorsoDamagePercent;
                break;
            case EnemyHitZone.HitZoneType.Leg:
                HitPointRaw -= velocity / 100 + LegDamagePercent;
                break;
            case EnemyHitZone.HitZoneType.Arm:
                HitPointRaw -= velocity / 100 + ArmDamagePercent;
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
        ragdoll.gameObject.AddComponent<DestroyObjectAfterSeconds>().TimeSecondToDestroy = 30f;
        ragdoll.gameObject.SetActive(true);
    }


}
