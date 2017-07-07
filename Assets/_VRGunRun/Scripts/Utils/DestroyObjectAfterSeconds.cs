//======= Copyright (c) Viet Kien Nguyen, All rights reserved. ===============
//
// Purpose: destroy objects after time to save performance
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObjectAfterSeconds : MonoBehaviour
{

    public float TimeSecondToDestroy = 3f;
    float elapsedTime = 0;

    private void Update()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime >= TimeSecondToDestroy)
        {
            Destroy(gameObject);
        }
    }
}
