//======= Copyright (c) Viet Kien Nguyen, All rights reserved. ===============
//
// Purpose: gameplay manager for gameplay variables
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int numberOfTargetLaunched;
    [SerializeField] private int numberOfTargetDestroyed;
    [SerializeField] private int numberOfTargetEvaded;
    [SerializeField] private int numberOfShotFired;
    [SerializeField] private int numberOfShotHit;
    [SerializeField] private int numberOfShotMissed;

    [SerializeField] private float evasionRatio;
    [SerializeField] private float accuracyRatio;
    [SerializeField] private float skillLevelIndex;
    [SerializeField] private string skillLevel;

    public Valve.VR.InteractionSystem.Player VRPlayer;

    public int NumberOfTargetLaunched
    {
        get { return numberOfTargetLaunched; }
        set { numberOfTargetLaunched = value; }
    }

    public int NumberOfTargetDestroyed
    {
        get { return numberOfTargetDestroyed; }
        set { numberOfTargetDestroyed = value; }
    }

    public int NumberOfTargetEvaded
    {
        get { return numberOfTargetEvaded; }
        set { numberOfTargetEvaded = value; }
    }

    public int NumberOfShotFired
    {
        get { return numberOfShotFired; }
        set { numberOfShotFired = value; }
    }

    public int NumberOfShotHit
    {
        get { return numberOfShotHit; }
        set { numberOfShotHit = value; }
    }

    public int NumberOfShotMissed
    {
        get { return numberOfShotMissed; }
        set { numberOfShotMissed = value; }
    }

    public float EvasionRatio
    {
        get { return evasionRatio; }
    }

    public float AccuracyRatio
    {
        get { return accuracyRatio; }
    }

    public string SkillLevel
    {
        get { return skillLevel; }
    }


    private void Update()
    {
        if (numberOfTargetLaunched > 0 && numberOfShotFired > 0)
        {
            evasionRatio = (float)numberOfTargetDestroyed / (float)numberOfTargetLaunched;
            accuracyRatio = (float)numberOfShotHit / (float)numberOfShotFired;

            if (accuracyRatio != 0)
            {
                skillLevelIndex = (evasionRatio + accuracyRatio); // * 0.5f;
                if (skillLevelIndex >= 1f)
                {
                    skillLevel = "Cheater!";
                    return;
                }
                else if (skillLevelIndex > 0.9f)
                {
                    skillLevel = "Master";
                    return;
                }
                else if (skillLevelIndex > 0.8f)
                {
                    skillLevel = "Expert";
                    return;
                }
                else if (skillLevelIndex > 0.6f)
                {
                    skillLevel = "Adept";
                    return;
                }
                else if (skillLevelIndex > 0.4f)
                {
                    skillLevel = "Skilled";
                    return;
                }
                else if (skillLevelIndex > 0.2f)
                {
                    skillLevel = "Average";
                    return;
                }
                else if (skillLevelIndex > 0f)
                {
                    skillLevel = "Noob";
                    return;
                }

            }
            else
            {
                skillLevel = "evaluating";
            }
        }
    }

    public void ResetStatistic()
    {
        numberOfTargetLaunched = 0;
        numberOfTargetDestroyed = 0;
        numberOfTargetEvaded = 0;
        numberOfShotFired = 0;
        numberOfShotHit = 0;
        numberOfShotMissed = 0;
    }

    public void ResetLauncher()
    {
        var launchers = FindObjectsOfType<TargetLauncher>();
        foreach (var launcher in launchers)
        {
            launcher.StartLaunchRate = 10f;
        }
    }
}