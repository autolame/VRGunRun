//======= Copyright (c) Viet Kien Nguyen, All rights reserved. ===============
//
// Purpose: GUI to show game score
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameScoreGUI : MonoBehaviour
{
    GameManager gameManager;
    [SerializeField] private Text shotFired, shotHit, shotMissed;
    [SerializeField] private Text targetLaunched, targetDestroyed, targetEvaded;
    [SerializeField] private Text evasionRatio, accuracyRatio, skillLevel;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        if (!gameManager)
        {
            Debug.LogWarning(gameObject.name + " CANNOT FIND GAME MANAGER");
        }
    }

    void Update()
    {
        shotFired.text = gameManager.NumberOfShotFired.ToString();
        shotHit.text = gameManager.NumberOfShotHit.ToString();
        shotMissed.text = gameManager.NumberOfShotMissed.ToString();

        targetLaunched.text = gameManager.NumberOfTargetLaunched.ToString();
        targetDestroyed.text = gameManager.NumberOfTargetDestroyed.ToString();
        targetEvaded.text = gameManager.NumberOfTargetEvaded.ToString();

        evasionRatio.text = gameManager.EvasionRatio.ToString("n2");
        accuracyRatio.text = gameManager.AccuracyRatio.ToString("n2");
        skillLevel.text = gameManager.SkillLevel;
    }
}
