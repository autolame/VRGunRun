//======= Copyright (c) Viet Kien Nguyen, All rights reserved. ===============
//
// Purpose: split input of touch pad into 7 zones
//          and handling it 
//
//=============================================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class TouchInputSevenZones : MonoBehaviour
{
    public Hand hand;
    private float midButtonRadius = 0.3f;
    public Vector2 touchPosition
    {
        get
        {
            if (hand)
                return hand.controller.GetAxis();
            else
                return Vector2.zero;
        }
    }
    public bool touched
    {
        get
        {
            if (touchPosition != Vector2.zero)
            { return true; }
            else { return false; }
        }
    }
    public float touchDistanceFromCenter
    {
        get { return touchPosition.magnitude; }
    }
    public float midSectionMin
    { get { return -midButtonRadius; } }
    public float midSectionMax
    { get { return midButtonRadius; } }
    public bool MidButtonHover
    {
        get
        {
            if (touched)
            {
                if (touchDistanceFromCenter < midButtonRadius && (touchPosition.x > midSectionMin && touchPosition.x < midSectionMax) && (touchPosition.y > midSectionMin && touchPosition.y < midSectionMax))
                { return true; }
                else { return false; }
            }
            else { return false; }
        }
    }
    public bool MidButtonPressed
    {
        get
        {
            if (touched)
            {
                if (MidButtonHover)
                {
                    if (hand.controller.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad))
                    { return true; }
                    else { return false; }
                }
                else { return false; }
            }
            else { return false; }
        }
    }
    public bool TopMidButtonHover
    {
        get
        {
            if (touched)
            {
                if (touchDistanceFromCenter > midSectionMax && (touchPosition.x > midSectionMin && touchPosition.x < midSectionMax) && (touchPosition.y > midSectionMin))
                { return true; }
                else { return false; }
            }
            else { return false; }
        }
    }
    public bool TopMidButtonPressed
    {
        get
        {
            if (touched)
            {
                if (TopMidButtonHover)
                {
                    if (hand.controller.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad))
                    { return true; }
                    else { return false; }
                }
                else { return false; }
            }
            else { return false; }
        }
    }
    public bool BotMidButtonHover
    {
        get
        {
            if (touched)
            {
                if (touchDistanceFromCenter > midSectionMax && (touchPosition.x > midSectionMin && touchPosition.x < midSectionMax) && (touchPosition.y < midSectionMin))
                { return true; }
                else { return false; }
            }
            else { return false; }
        }
    }
    public bool BotMidButtonPressed
    {
        get
        {
            if (touched)
            {
                if (BotMidButtonHover)
                {
                    if (hand.controller.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad))
                    { return true; }
                    else { return false; }
                }
                else { return false; }
            }
            else { return false; }
        }
    }
    public bool TopLeftButtonHover
    {
        get
        {
            if (touched)
            {
                if (touchDistanceFromCenter > midSectionMax && (touchPosition.x < midSectionMax) && (touchPosition.y > midSectionMax))
                { return true; }
                else { return false; }
            }
            else { return false; }
        }
    }
    public bool TopLeftButtonPressed
    {
        get
        {
            if (touched)
            {
                if (TopLeftButtonHover)
                {
                    if (hand.controller.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad))
                    { return true; }
                    else { return false; }
                }
                else { return false; }
            }
            else { return false; }
        }
    }
    public bool TopRightButtonHover
    {
        get
        {
            if (touched)
            {
                if (touchDistanceFromCenter > midSectionMax && (touchPosition.x > midSectionMax) && (touchPosition.y > midSectionMax))
                { return true; }
                else { return false; }
            }
            else { return false; }
        }
    }
    public bool TopRightButtonPressed
    {
        get
        {
            if (touched)
            {
                if (TopRightButtonHover)
                {
                    if (hand.controller.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad))
                    { return true; }
                    else { return false; }
                }
                else { return false; }
            }
            else { return false; }
        }
    }
    public bool BotRightButtonHover
    {
        get
        {
            if (touched)
            {
                if (touchDistanceFromCenter > midSectionMax && (touchPosition.x < midSectionMax) && (touchPosition.y < midSectionMax))
                { return true; }
                else { return false; }
            }
            else { return false; }
        }
    }
    public bool BotRightButtonPressed
    {
        get
        {
            if (touched)
            {
                if (BotRightButtonHover)
                {
                    if (hand.controller.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad))
                    { return true; }
                    else { return false; }
                }
                else { return false; }
            }
            else { return false; }
        }
    }
    public bool BotLeftButtonHover
    {
        get
        {
            if (touched)
            {
                if (touchDistanceFromCenter > midSectionMax && (touchPosition.x > midSectionMax) && (touchPosition.y < midSectionMax))
                { return true; }
                else { return false; }
            }
            else { return false; }
        }
    }
    public bool BotLeftButtonPressed
    {
        get
        {
            if (touched)
            {
                if (MidButtonHover)
                {
                    if (hand.controller.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad))
                    { return true; }
                    else { return false; }
                }
                else { return false; }
            }
            else { return false; }
        }
    }
}
