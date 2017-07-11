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

    private HandItemManager HandItemManager1;
    private HandItemManager HandItemManager2;


    [EnumFlags]
    public Hand.AttachmentFlags attachmentFlags = Hand.defaultAttachmentFlags;


    public List<GameObject> cleanUpListHand2 = new List<GameObject>();

    [SerializeField] private EmptyHand emptyHand;
    [SerializeField] private List<Gun> gunList = new List<Gun>();

    private void Awake()
    {
        HandItemManager1 = hand1.gameObject.AddComponent<HandItemManager>();
        HandItemManager2 = hand2.gameObject.AddComponent<HandItemManager>();
        HandItemManager1.GunList = gunList;
        HandItemManager2.GunList = gunList;
        HandItemManager1.EmptyHand = emptyHand;
        HandItemManager2.EmptyHand = emptyHand;
        HandItemManager1.AttachmentFlags = attachmentFlags;
        HandItemManager2.AttachmentFlags = attachmentFlags;
    }
}
