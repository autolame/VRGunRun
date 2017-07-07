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

    public Hand activeHand;
    private GamePlayerManager gamePlayerManager;


    [SerializeField] private Text remainingBulletCountText;
    public int remainingBulletCount;
    [SerializeField] private Text autoFireText;
    public bool autoFire;

    public GameObject touchPointer;
    [SerializeField] float touchPointerMaxRange = 0.3f;

    [SerializeField] private Button[] buttons = new Button[7];
    public int highlightedBtnIndex = -1;
    public bool highlightedBtnPressed = false;

    private void Awake()
    {
        gamePlayerManager = FindObjectOfType<GamePlayerManager>();
    }

    private void Start()
    {
        gamePlayerManager = FindObjectOfType<GamePlayerManager>();

    }

    private void Update()
    {
        // TODO check if ever need to NULLCHECK activeHand

        //remainingBulletCountText.text = remainingBulletCount.ToString();
        //if (remainingBulletCount > 0)
        //{
        //    autoFireText.text = autoFire ? "auto" : "single";
        //}
        //else
        //{
        //    autoFireText.text = "reload";
        //}

        if (highlightedBtnIndex >= 0 && highlightedBtnIndex < buttons.Length)
        {
            if (highlightedBtnPressed)
            {
                buttons[highlightedBtnIndex].onClick.Invoke();
                highlightedBtnPressed = false;
            }
            else
            {
                buttons[highlightedBtnIndex].Select();
            }
        }
    }

    public void MoveIndexPointerTo(Vector2 position)
    {
        touchPointer.SetActive(true);
        //Vector2 newPosition = new Vector2(Mathf.Clamp(-touchPointerMaxRange, touchPointerMaxRange, position.x), Mathf.Clamp(-touchPointerMaxRange, touchPointerMaxRange, position.y));
        touchPointer.transform.localPosition = position * touchPointerMaxRange;
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
