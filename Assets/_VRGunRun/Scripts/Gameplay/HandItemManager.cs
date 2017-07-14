using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class HandItemManager : MonoBehaviour
{
    private Hand hand;

    public EmptyHand EmptyHand;
    public EmptyHand attachedEmptyHand;
    public List<Gun> GunList = new List<Gun>();
    public List<Gun> attachedGunList = new List<Gun>();

    public Gun GunInHand;
    public bool IsTeleporting;

    private bool firstTimeSwitch = true;

    public int ActiveItemIndex;

    //public List<GameObject> cleanUpListHand = new List<GameObject>();

    public Hand.AttachmentFlags AttachmentFlags;

    private void Awake()
    {
        hand = GetComponent<Hand>();
    }

    private void AttachItemsToHand()
    {
        if (firstTimeSwitch)
        {
            EmptyHand spawnedEmptyHand = Instantiate(EmptyHand);
            hand.AttachObject(spawnedEmptyHand.gameObject, AttachmentFlags, "");
            attachedEmptyHand = spawnedEmptyHand;
            foreach (Gun gun in GunList)
            {
                Gun spawnedGun = Instantiate(gun);
                hand.AttachObject(spawnedGun.gameObject, AttachmentFlags, "");
                attachedGunList.Add(spawnedGun);
            }
            firstTimeSwitch = false;
        }
    }
    public void SwitchToNextGun(Hand hand)
    {
        AttachItemsToHand();

        if (ActiveItemIndex < attachedGunList.Count - 1)
        { ActiveItemIndex++; }
        else
        { ActiveItemIndex = 0; }

        HideAttachedItems(hand);
        attachedGunList[ActiveItemIndex].gameObject.SetActive(true);
        //hand.AttachObject(attachedGunList[ActiveItemIndex].gameObject, AttachmentFlags, "");
        //SpawnItemAndAttachToHand(hand);
    }
    public void SwitchToPrevGun(Hand hand)
    {
        AttachItemsToHand();

        if (ActiveItemIndex > 0)
        { ActiveItemIndex--; }
        else
        { ActiveItemIndex = attachedGunList.Count - 1; }

        HideAttachedItems(hand);
        attachedGunList[ActiveItemIndex].gameObject.SetActive(true);
        //hand.AttachObject(attachedGunList[ActiveItemIndex].gameObject, AttachmentFlags, "");
        //SpawnItemAndAttachToHand(hand);
    }

    public void SwitchToEmptyHand(Hand hand)
    {
        HideAttachedItems(hand);
        attachedEmptyHand.gameObject.SetActive(true);
        hand.AttachObject(attachedEmptyHand.gameObject, AttachmentFlags, "");
    }

    private void HideAttachedItems(Hand hand)
    {
        attachedEmptyHand.gameObject.SetActive(false);
        foreach (Gun gun in attachedGunList)
        {
            gun.gameObject.SetActive(false);
        }
    }

    //public void QueueForCleanUp(GameObject gameObject)
    //{
    //    cleanUpListHand.Add(gameObject);
    //}

    //public void CleanHand(Hand hand)
    //{
    //    foreach (var go in cleanUpListHand)
    //    {
    //        Destroy(go);
    //    }
    //    cleanUpListHand.Clear();
    //}

    //public void SpawnItemAndAttachToHand(Hand hand)
    //{
    //    var spawnedItem = Instantiate(GunList[ActiveItemIndex]);
    //    hand.AttachObject(spawnedItem.gameObject, AttachmentFlags, "");
    //    spawnedItem.gameObject.SetActive(true);
    //    // TODO check if cleanup is possible when switching weapon
    //}

    //public void EmptyGunHand(Hand hand)
    //{
    //    var emptyItem = Instantiate(EmptyHand);
    //    hand.AttachObject(emptyItem.gameObject, AttachmentFlags, "");
    //    emptyItem.gameObject.SetActive(true);
    //    CleanHand(hand);
    //}


}
