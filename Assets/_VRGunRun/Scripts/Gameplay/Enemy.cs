using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class Enemy : MonoBehaviour
{
    public float HitPoint = 100f;

    public EnemyRagdoll ragdollController;

    public float HeadDamagePercent = 100f;
    public float TorsoDamagePercent = 20f;
    public float LegDamagePercent = 10f;
    public float ArmDamagePercent = 5f;

    public int AnimState = 0;

    public EnemyGoal Goal;
    private NavMeshAgent agent;
    private Animator animator;

    private void Awake()
    {
        Goal = FindObjectOfType<EnemyGoal>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        MoveTowardsGoal();
    }

    private void Start()
    {
        AnimState = Random.Range(1, 4);
        animator.SetInteger("AnimState", AnimState);
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
            if (HitPoint > 0)
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
                HitPoint -= velocity / 100 + HeadDamagePercent;
                break;
            case EnemyHitZone.HitZoneType.Torso:
                HitPoint -= velocity / 100 + TorsoDamagePercent;
                break;
            case EnemyHitZone.HitZoneType.Leg:
                HitPoint -= velocity / 100 + LegDamagePercent;
                break;
            case EnemyHitZone.HitZoneType.Arm:
                HitPoint -= velocity / 100 + ArmDamagePercent;
                break;
        }
    }

    public void SpawnRagdoll()
    {
        var ragdoll = Instantiate(ragdollController);
        ragdoll.transform.position = transform.position;
        ragdoll.transform.rotation = transform.rotation;
        ragdoll.Enemy = this;
        ragdoll.GetRagdollPose();
        ragdoll.PoseRagdoll();
        ragdoll.gameObject.AddComponent<DestroyObjectAfterSeconds>().TimeSecondToDestroy = 30f;
        ragdoll.gameObject.SetActive(true);
    }


}
