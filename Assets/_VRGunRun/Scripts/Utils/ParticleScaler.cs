//======= Copyright (c) Viet Kien Nguyen, All rights reserved. ===============
//
// Purpose: scale the particle
//
//=============================================================================

using UnityEngine;

public class ParticleSystemMultiplier : MonoBehaviour
{

    // a simple script to scale the size, speed and lifetime of a particle system

    public float Multiplier = 1;

    private void Start()
    {
        var systems = GetComponentsInChildren<ParticleSystem>();
        foreach (ParticleSystem system in systems)
        {
            system.startSize *= Multiplier;
            system.startSpeed *= Multiplier;
            //system.startLifetime *= Mathf.Lerp(Multiplier, 1, 0.5f);
            system.Clear();
            system.Play();
        }
    }
}
