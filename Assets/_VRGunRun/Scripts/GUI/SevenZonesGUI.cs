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

public class SevenZonesGUI : MonoBehaviour
{
    // TODO make hand directly linked to gui and playerController instead of communicating through GUI

    public Hand activeHand;
    private GamePlayerManager gamePlayerManager;
    private TouchInputSevenZones touchInputSevenZones;

    public GameObject touchPointer;
    [SerializeField] float touchPointerMaxRange = 0.3f;

    [SerializeField] private Button[] buttons = new Button[7];


    private void Start()
    {
        gamePlayerManager = FindObjectOfType<GamePlayerManager>();
        touchInputSevenZones = gameObject.AddComponent<TouchInputSevenZones>();
        touchInputSevenZones.hand = activeHand;
    }
    private int TouchedButtonIndex
    {
        get
        {
            if (touchInputSevenZones.MidButtonHover)
            {
                //Debug.Log("MidButtonHover");
                return 0;
            }
            else if (touchInputSevenZones.TopMidButtonHover)
            {
                //Debug.Log("TopMidButtonHover");
                return 1;
            }
            else if (touchInputSevenZones.TopRightButtonHover)
            {
                //Debug.Log("BotMidButtonHover");
                return 2;
            }
            else if (touchInputSevenZones.BotRightButtonHover)
            {
                //Debug.Log("TopLeftButtonHover");
                return 3;
            }
            else if (touchInputSevenZones.BotMidButtonHover)
            {
                //Debug.Log("TopRightButtonHover");
                return 4;
            }
            else if (touchInputSevenZones.BotLeftButtonHover)
            {
                //Debug.Log("BotRightButtonHover");
                return 5;
            }
            else if (touchInputSevenZones.TopLeftButtonHover)
            {
                //Debug.Log("BotLeftButtonHover");
                return 6;
            }
            else
            {
                return -1;
            }
        }
    }
    private int PressedButtonIndex
    {
        get
        {
            if (touchInputSevenZones.MidButtonPressed)
            {
                //Debug.Log("MidButtonPressed");
                return 0;
            }
            else if (touchInputSevenZones.TopMidButtonPressed)
            {
                //Debug.Log("TopMidButtonPressed");
                return 1;
            }
            else if (touchInputSevenZones.TopRightButtonPressed)
            {
                //Debug.Log("BotMidButtonPressed");
                return 2;
            }
            else if (touchInputSevenZones.BotRightButtonPressed)
            {
                //Debug.Log("TopLeftButtonPressed");
                return 3;
            }
            else if (touchInputSevenZones.BotMidButtonPressed)
            {
                //Debug.Log("TopRightButtonPressed");
                return 4;
            }
            else if (touchInputSevenZones.BotLeftButtonPressed)
            {
                //Debug.Log("BotRightButtonPressed");
                return 5;
            }
            else if (touchInputSevenZones.TopLeftButtonPressed)
            {
                //Debug.Log("BotLeftButtonPressed");
                return 6;
            }
            else
            { return -1; }
        }
    }

    private void Update()
    {
        UpdateTouchPointer();
        // highlight the hovered button
        if (TouchedButtonIndex != -1 && TouchedButtonIndex < buttons.Length)
        {
            buttons[TouchedButtonIndex].Select();
        }

        if (PressedButtonIndex != -1 && TouchedButtonIndex < buttons.Length)
        {
            buttons[TouchedButtonIndex].onClick.Invoke();
        }

        if (touchInputSevenZones.touched)
        { GetComponent<Canvas>().enabled = true; }
        else
        { GetComponent<Canvas>().enabled = false; }
    }

    public void UpdateTouchPointer()
    {
        if (touchInputSevenZones.touched)
        {
            touchPointer.SetActive(true);
            touchPointer.transform.localPosition = touchInputSevenZones.touchPosition * touchPointerMaxRange;
        }
        else
        {
            touchPointer.SetActive(false);
        }
    }

    // those methods will be used in the editor to trigger button functions
    // TODO make the button actually have functions
    public bool MidBtnPressed()
    {
        return touchInputSevenZones.MidButtonPressed;
    }
    public bool TopMidBtnPressed()
    {
        return touchInputSevenZones.TopMidButtonPressed;
    }
    public bool BotMidBtnPressed()
    {
        return touchInputSevenZones.BotMidButtonPressed;
    }
    public bool TopLeftBtnPressed()
    {
        return touchInputSevenZones.TopLeftButtonPressed;
    }
    public bool TopRightBtnPressed()
    {
        return touchInputSevenZones.TopRightButtonPressed;
    }
    public bool BotLeftBtnPressed()
    {
        return touchInputSevenZones.BotLeftButtonPressed;
    }
    public bool BotRightBtnPressed()
    {
        return touchInputSevenZones.BotRightButtonPressed;
    }
}
