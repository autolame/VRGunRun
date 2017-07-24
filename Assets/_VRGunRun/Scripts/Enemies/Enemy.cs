﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public float StartHitPointRaw = 100f; // default hp for this enemy
    public float StartMovementSpeedRaw = 1f;  // default movement speed for this enemy

    float currentHitPointPercentage;
    float currentMovementSpeedPercentage;
    float currentHitPointRaw;
    float currentMovementSpeedRaw;

    public EnemyGoal Goal;
    public NavMeshAgent NavAgent;
    public bool IsDestroyed
    {
        get
        {
            if (currentHitPointRaw > 0)
                return false;
            else
                return true;
        }
    }

    public float CurrentHitPointPercentage
    {
        get
        {
            return 100f - ((StartHitPointRaw - currentHitPointRaw) / 100f);
        }
    }

    public float CurrentMovementSpeedPercentage
    {
        get
        {
            return 100f - ((StartMovementSpeedRaw - currentMovementSpeedRaw) / 100f);
        }
    }

    protected void Awake()
    {
        currentHitPointRaw = StartHitPointRaw;
        currentHitPointPercentage = StartMovementSpeedRaw;

        NavAgent = GetComponent<NavMeshAgent>();
    }
    public void MoveTowardsGoal()
    {
        NavAgent.destination = Goal.Position;
    }
    protected void StatusUpdate()
    {
        currentHitPointPercentage = CurrentHitPointPercentage;
        currentMovementSpeedPercentage = CurrentMovementSpeedPercentage;
    }
    public void DamageRawHitPoint(float damage)
    {
        currentHitPointRaw -= damage;
        //StatusUpdate();
    }
    public void DamageRawMovementSpeed(float slow)
    {
        currentMovementSpeedRaw -= slow;
        //StatusUpdate();
    }
    public void DamagePercentageHitPoint(float percent)
    {
        DamageRawHitPoint(percent * (StartHitPointRaw / 100f));
        //StatusUpdate();
    }
    public void DamagePercentageMovementSpeed(float slowPercent)
    {
        DamageRawMovementSpeed(slowPercent * (StartMovementSpeedRaw / 100f));
        //StatusUpdate();
    }
}
