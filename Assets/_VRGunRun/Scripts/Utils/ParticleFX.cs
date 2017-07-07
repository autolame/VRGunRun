//======= Copyright (c) Viet Kien Nguyen, All rights reserved. ===============
//
// Purpose: particle spawner
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleFX : MonoBehaviour
{
    public float LifeTime = 1;
    float elapsedTime = 0;

    private void Awake()
    {
        elapsedTime = 0;
    }
    private void Start()
    {
        elapsedTime = 0;
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime > LifeTime)
        {
            Destroy(gameObject);
        }
    }

    public ParticleFX SpawnAt(Vector3 position, float lifeTime)
    {
        ParticleFX newFX = Instantiate(this);
        newFX.LifeTime = lifeTime;
        newFX.transform.position = position;

        return newFX;
    }
}
