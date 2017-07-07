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
    [SerializeField] private Hand hand1;
    [SerializeField] private Hand hand2;

    public int activeItemIndex;
    [EnumFlags]
    public Hand.AttachmentFlags attachmentFlags = Hand.defaultAttachmentFlags;

    public List<GameObject> cleanUpList = new List<GameObject>();

    [SerializeField] private EmptyHand emptyHand;
    [SerializeField] private List<Gun> gunList = new List<Gun>();

    public void IncreaseItemIndex()
    {
        if (activeItemIndex < gunList.Count - 1)
        { activeItemIndex++; }
        else
        { activeItemIndex = 0; }
    }
    public void DecreaseItemIndex()
    {
        if (activeItemIndex > 0)
        { activeItemIndex--; }
        else
        { activeItemIndex = gunList.Count - 1; }
    }
    public void QueueForCleanUp(GameObject gameObject)
    {
        cleanUpList.Add(gameObject);
    }
    public void CleanHand(Hand hand)
    {
        foreach (var go in cleanUpList)
        {
            Destroy(go);
        }
    }
    public void SpawnItemAndAttachToHand(Hand hand)
    {
        var newItem = Instantiate(gunList[activeItemIndex]);
        hand.AttachObject(newItem.gameObject, attachmentFlags, "");
        newItem.gameObject.SetActive(true);

        CleanHand(hand);
    }

    public void EmptyHand(Hand hand)
    {
        var newItem = Instantiate(emptyHand);
        hand.AttachObject(newItem.gameObject, attachmentFlags, "");
        newItem.gameObject.SetActive(true);

        CleanHand(hand);
    }


}
