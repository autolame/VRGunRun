//======= Copyright (c) Viet Kien Nguyen, All rights reserved. ===============
//
// Purpose: manager for the gameplay interaction of player
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;
using Valve.VR;

public class GamePlayerManager : MonoBehaviour
{
    public Hand activeHand;
    public int activeItemIndex;

    [SerializeField] private List<Gun> gunList = new List<Gun>();



    public Hand ActiveHand
    {
        get { return activeHand; }
        set { activeHand = value; }
    }

    public void SetActiveHand(Hand hand)
    {
        activeHand = hand;
    }
    public void IncreaseItemIndex()
    {
        if (activeItemIndex < gunList.Count - 1)
        {
            activeItemIndex++;
        }
        else
        {
            activeItemIndex = 0;
        }
    }
    public void DecreaseItemIndex()
    {
        if (activeItemIndex < gunList.Count - 1)
        {
            activeItemIndex--;
        }
        else
        {
            activeItemIndex = gunList.Count - 1;
        }
    }

    public void AttachItemIntoHand()
    {
        // TODO Straighten this out
        var itemToAttach = Instantiate(gunList[activeItemIndex].gameObject);
        activeHand.AttachObject(itemToAttach, Hand.defaultAttachmentFlags, string.Empty);
    }

}
