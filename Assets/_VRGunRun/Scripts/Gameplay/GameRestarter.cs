//======= Copyright (c) Viet Kien Nguyen, All rights reserved. ===============
//
// Purpose: shoot this to restart the stats counter
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRestarter : MonoBehaviour
{
    private GameManager gameManager;
    private int hitPoint = 3;
    private int hitCount = 0;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        if (!gameManager)
        {
            Debug.LogWarning(gameObject.name + " CANNOT FIND GAME MANAGER");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.GetComponent<GunAmmoBullet>())
        {
            if (hitCount >= hitPoint)
            {
                gameManager.ResetStatistic();
                hitCount = 0;
            }
            float lerp = (float)hitCount / (float)hitPoint;
            GetComponent<Renderer>().material.color = Color.Lerp(Color.green, Color.red, lerp);
            hitCount++;
        }
    }
}
