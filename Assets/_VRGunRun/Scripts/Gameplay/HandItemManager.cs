using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class HandItemManager : MonoBehaviour
{
    private Hand hand;

    public EmptyHand EmptyHand;
    public GameObject PrevItemInHand;
    public bool IsTeleporting;
    public List<Gun> GunList = new List<Gun>();

    public int ActiveItemIndex;

    public List<GameObject> cleanUpListHand = new List<GameObject>();

    public Hand.AttachmentFlags AttachmentFlags;

    private void Awake()
    {
        hand = GetComponent<Hand>();
    }
    public void SwitchToNextGun(Hand hand)
    {
        QueueForCleanUp(hand);

        if (ActiveItemIndex < GunList.Count - 1)
        { ActiveItemIndex++; }
        else
        { ActiveItemIndex = 0; }

        SpawnItemAndAttachToHand(hand);
    }
    public void SwitchToPrevGun(Hand hand)
    {
        QueueForCleanUp(hand);

        if (ActiveItemIndex > 0)
        { ActiveItemIndex--; }
        else
        { ActiveItemIndex = GunList.Count - 1; }

        SpawnItemAndAttachToHand(hand);
    }

    public void QueueForCleanUp(Hand hand)
    {
        if (hand.currentAttachedObject)
        {
            cleanUpListHand.Add(hand.currentAttachedObject);
            PrevItemInHand = hand.currentAttachedObject;
        }
        else
        {
            Debug.Log("tried to add NULL to cleanup list from hand: " + hand.name);
        }

    }
    // TODO fix hand switch item bug
    public void CleanUpHand(Hand hand)
    {

        //var newPrevItem = Instantiate(PrevItemInHand);

        foreach (var item in cleanUpListHand)
        {
            //if (item)
            //{
            //    if (item.activeSelf == false)
            //    {
            Destroy(item);
            //    }
            //}
        }

        cleanUpListHand.Clear();

        //hand.AttachObject(newPrevItem, AttachmentFlags, "");
    }

    public void SpawnItemAndAttachToHand(Hand hand)
    {
        var spawnedItem = Instantiate(GunList[ActiveItemIndex]);
        hand.AttachObject(spawnedItem.gameObject, AttachmentFlags, "");
        spawnedItem.gameObject.SetActive(true);
        // TODO check if cleanup is possible when switching weapon
        spawnedItem.name = spawnedItem + hand.name;
        //CleanHand(hand);
    }

    public void EmptyGunHand(Hand hand)
    {
        var emptyItem = Instantiate(EmptyHand);
        hand.AttachObject(emptyItem.gameObject, AttachmentFlags, "");
        emptyItem.gameObject.SetActive(true);
        emptyItem.name = emptyItem + hand.name;
        CleanUpHand(hand);
    }


}
