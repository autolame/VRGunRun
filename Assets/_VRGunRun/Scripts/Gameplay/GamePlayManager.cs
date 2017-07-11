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

public class GamePlayManager : MonoBehaviour
{
    [SerializeField] private Hand hand1;
    [SerializeField] private Hand hand2;

    public int activeItemIndex;
    [EnumFlags]
    public Hand.AttachmentFlags attachmentFlags = Hand.defaultAttachmentFlags;

    public List<GameObject> cleanUpListHand1 = new List<GameObject>();
    public List<GameObject> cleanUpListHand2 = new List<GameObject>();

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
    public void QueueForCleanUp(GameObject gameObject, Hand hand)
    {
        if (hand == hand1)
        {
            cleanUpListHand1.Add(gameObject);
        }

        if (hand == hand2)
        {
            cleanUpListHand2.Add(gameObject);
        }
    }
    public void CleanHand1(Hand hand)
    {
        foreach (var go in cleanUpListHand1)
        {
            Destroy(go);
        }

        cleanUpListHand1.Clear();
    }
    public void CleanHand2(Hand hand)
    {
        foreach (var go in cleanUpListHand2)
        {
            Destroy(go);
        }

        cleanUpListHand2.Clear();
    }
    public void SpawnItemAndAttachToHand(Hand hand)
    {
        var spawnedItem = Instantiate(gunList[activeItemIndex]);
        hand.AttachObject(spawnedItem.gameObject, attachmentFlags, "");
        spawnedItem.gameObject.SetActive(true);

        Debug.Log(hand.name + " : " + spawnedItem.name);
    }

    public void EmptyHand(Hand hand)
    {
        var emptyItem = Instantiate(emptyHand);
        hand.AttachObject(emptyItem.gameObject, attachmentFlags, "");
        emptyItem.gameObject.SetActive(true);

        Debug.Log(hand.name + " : empty hand");

        if (hand == hand1)
        {
            CleanHand1(hand);
        }

        if (hand == hand2)
        {
            CleanHand2(hand);
        }
    }
}
