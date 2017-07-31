//======= Copyright (c) Viet Kien Nguyen, All rights reserved. ===============
//
// Purpose: practice targets
//
//=============================================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetDummy : MonoBehaviour
{
    private GameManager gameManager;

    [SerializeField] ParticleFX explosionFX;
    public float Health = 1000f;


    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        if (!gameManager)
        {
            Debug.LogWarning(gameObject.name + " CANNOT FIND GAME MANAGER");
        }
        else
        {
            gameManager.NumberOfTargetLaunched++;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<GunAmmoBullet>())
        {
            Health -= collision.gameObject.GetComponent<Rigidbody>().velocity.magnitude;
        }
        else
        {
            gameManager.NumberOfTargetEvaded++;
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (Health <= 0)
        {
            ParticleFX newFX = explosionFX.SpawnAtTransform(transform, 10f);
            gameManager.NumberOfTargetDestroyed++;
            FindObjectOfType<SlomoManager>().StartSlomoFor(5);
            Destroy(gameObject);
        }
    }
}
