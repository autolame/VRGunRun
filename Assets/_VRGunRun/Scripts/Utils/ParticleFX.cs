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
    public ParticleFX SpawnAtTransform(Transform parentTransform, float lifeTime)
    {
        ParticleFX newFX = Instantiate(this);
        newFX.transform.position = parentTransform.position;
        newFX.transform.rotation = parentTransform.rotation;
        Destroy(newFX.gameObject, lifeTime);
        return newFX;
    }
    public ParticleFX SpawnAtPosition(Vector3 position, float lifeTime)
    {
        ParticleFX newFX = Instantiate(this);
        newFX.transform.position = position;
        Destroy(newFX.gameObject, lifeTime);
        return newFX;
    }
}
