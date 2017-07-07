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
    public bool highlightedBtnPressed = false;

    private void Start()
    {
        gamePlayerManager = FindObjectOfType<GamePlayerManager>();
        touchInputSevenZones = gameObject.AddComponent<TouchInputSevenZones>();
        touchInputSevenZones.hand = activeHand;
    }

    private void Update()
    {
        UpdateTouchPointer();
        // highlight the hovered button
        if (TouchedButtonIndex >= 0 && TouchedButtonIndex < buttons.Length)
        {
            if (highlightedBtnPressed)
            {
                buttons[TouchedButtonIndex].onClick.Invoke();
                highlightedBtnPressed = false;
            }
            else
            {
                buttons[TouchedButtonIndex].Select();
            }
        }
        else
        {
            // TODO deselect button
            foreach (var button in buttons)
            {

            }
        }
    }

    private int TouchedButtonIndex
    {
        get
        {
            if (touchInputSevenZones.MidButtonHover)
            { return 0; }
            else if (touchInputSevenZones.TopMidButtonHover)
            { return 1; }
            else if (touchInputSevenZones.BotMidButtonHover)
            { return 2; }
            else if (touchInputSevenZones.TopLeftButtonHover)
            { return 3; }
            else if (touchInputSevenZones.TopRightButtonHover)
            { return 4; }
            else if (touchInputSevenZones.BotRightButtonHover)
            { return 5; }
            else if (touchInputSevenZones.BotLeftButtonHover)
            { return 6; }
            else
            { return -1; }
        }
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
    public void MidBtnPressed()
    {

    }
    public void TopMidBtnPressed()
    {

    }
    public void BotMidBtnPressed()
    {

    }
    public void TopLeftBtnPressed()
    {
        // TODO test 
        gamePlayerManager.DecreaseItemIndex();
        gamePlayerManager.AttachItemIntoHand();
    }
    public void TopRightBtnPressed()
    {
        // TODO test
        gamePlayerManager.IncreaseItemIndex();

    }
    public void BotLeftBtnPressed()
    {

    }
    public void BotRightBtnPressed()
    {

    }

}
