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
    public ParticleFX SpawnAt(Vector3 position, float lifeTime)
    {
        ParticleFX newFX = Instantiate(this);
        newFX.transform.position = position;
        Destroy(newFX.gameObject, lifeTime);
        return newFX;
    }
}
