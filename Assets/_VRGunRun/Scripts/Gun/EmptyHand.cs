﻿//======= Copyright (c) Viet Kien Nguyen, All rights reserved. ===============
//
// Purpose: The empty hand
//
//=============================================================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Valve.VR.InteractionSystem;
using Valve.VR;


//-------------------------------------------------------------------------------------------------
[RequireComponent(typeof(Interactable))]
public class EmptyHand : MonoBehaviour
{
    public enum Handedness { Left, Right };
    public Handedness currentHandGuess = Handedness.Right;

    private Hand hand;
    [SerializeField] private SevenZonesGUI sevenZonesGUI;
    [SerializeField] private Transform guiTransform;

    private HandItemManager handItemManager;

    private SteamVR_Events.Action newPosesAppliedAction;

    //-------------------------------------------------------------------------------------------------
    private void OnAttachedToHand(Hand attachedHand)
    {
        hand = attachedHand;
        if (sevenZonesGUI && hand)
            sevenZonesGUI.activeHand = hand;
    }
    //-------------------------------------------------------------------------------------------------
    private void Awake()
    {
        newPosesAppliedAction = SteamVR_Events.NewPosesAppliedAction(OnNewPosesApplied);
    }
    //-------------------------------------------------------------------------------------------------
    private void Start()
    {
        handItemManager = hand.GetComponent<HandItemManager>();
    }
    //-------------------------------------------------------------------------------------------------
    void OnEnable()
    {
        newPosesAppliedAction.enabled = true;
    }
    //-------------------------------------------------------------------------------------------------
    void OnDisable()
    {
        newPosesAppliedAction.enabled = false;
    }
    //-------------------------------------------------------------------------------------------------
    void LateUpdate()
    {

    }
    //-------------------------------------------------------------------------------------------------
    private void OnNewPosesApplied()
    {

    }
    //-------------------------------------------------------------------------------------------------

    //-------------------------------------------------------------------------------------------------
    private void HandAttachedUpdate(Hand hand)
    {
        if (sevenZonesGUI.TopRightBtnPressed())
        {
            handItemManager.QueueForCleanUp(gameObject);
            handItemManager.SwitchToNextGun();
            handItemManager.SpawnItemAndAttachToHand();
        }

        if (sevenZonesGUI.TopLeftBtnPressed())
        {
            handItemManager.QueueForCleanUp(gameObject);
            handItemManager.SwitchToPrevGun();
            handItemManager.SpawnItemAndAttachToHand();
        }
    }
    //-------------------------------------------------------------------------------------------------

    //-------------------------------------------------------------------------------------------------

    //-------------------------------------------------------------------------------------------------

    //-------------------------------------------------------------------------------------------------
    private void ShutDown()
    {
        if (hand != null && hand.otherHand.currentAttachedObject != null)
        {
            if (hand.otherHand.currentAttachedObject.GetComponent<ItemPackageReference>() != null)
            {
                //if (hand.otherHand.currentAttachedObject.GetComponent<ItemPackageReference>().itemPackage == magazineHandItemPackage)
                //{
                hand.otherHand.DetachObject(hand.otherHand.currentAttachedObject);
                //}
            }
        }
    }
    //-------------------------------------------------------------------------------------------------
    private void OnHandFocusLost(Hand hand)
    {
        gameObject.SetActive(false);
    }
    //-------------------------------------------------------------------------------------------------
    private void OnHandFocusAcquired(Hand hand)
    {
        gameObject.SetActive(true);
        OnAttachedToHand(hand);
    }
    //-------------------------------------------------------------------------------------------------
    private void OnDetachedFromHand(Hand hand)
    {
        Destroy(gameObject);
    }
    //-------------------------------------------------------------------------------------------------
    void OnDestroy()
    {
        ShutDown();
    }
}
