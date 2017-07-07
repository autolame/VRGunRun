//======= Copyright (c) Viet Kien Nguyen, All rights reserved. ===============
//
// Purpose: HUD attached on the Gun
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR.InteractionSystem;

public class GunGUI : MonoBehaviour
{
    // TODO make hand directly linked to gui and playerController instead of communicating through GUI

    [SerializeField] private Text remainingBulletCountText;
    public int remainingBulletCount;
    [SerializeField] private Text autoFireText;
    public bool autoFire;

    private void Update()
    {
        remainingBulletCountText.text = remainingBulletCount.ToString();
        if (remainingBulletCount > 0)
        {
            autoFireText.text = autoFire ? "auto" : "single";
        }
        else
        {
            autoFireText.text = "reload";
        }
    }
}
