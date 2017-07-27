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
    public ParticleFX SpawnAt(Transform parentTransform, float lifeTime)
    {
        ParticleFX newFX = Instantiate(this);
        newFX.transform.position = parentTransform.position;
        newFX.transform.rotation = parentTransform.rotation;
        Destroy(newFX.gameObject, lifeTime);
        return newFX;
    }
}
