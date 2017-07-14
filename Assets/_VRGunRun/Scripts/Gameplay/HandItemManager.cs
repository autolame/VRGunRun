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
        PrevItemInHand = hand.currentAttachedObject;
        Debug.Log(PrevItemInHand.name);
        //QueueForCleanUp(hand);

        if (ActiveItemIndex < GunList.Count - 1)
        { ActiveItemIndex++; }
        else
        { ActiveItemIndex = 0; }

        SpawnItemAndAttachToHand(hand);
    }
    public void SwitchToPrevGun(Hand hand)
    {
        PrevItemInHand = hand.currentAttachedObject;
        Debug.Log(PrevItemInHand.name);
        //QueueForCleanUp(hand);

        if (ActiveItemIndex > 0)
        { ActiveItemIndex--; }
        else
        { ActiveItemIndex = GunList.Count - 1; }

        SpawnItemAndAttachToHand(hand);
    }

    public void QueueForCleanUp(Hand hand)
    {
        cleanUpListHand.Add(hand.currentAttachedObject);
    }

    public void CleanHand(Hand hand)
    {
        Debug.Log("Destroying : " + PrevItemInHand.name);

        Destroy(PrevItemInHand);

        //foreach (var go in cleanUpListHand)
        //{
        //    Destroy(go);
        //}
        //cleanUpListHand.Clear();

        //EmptyGunHand(hand);
    }

    public void SpawnItemAndAttachToHand(Hand hand)
    {
        var spawnedItem = Instantiate(GunList[ActiveItemIndex]);
        hand.AttachObject(spawnedItem.gameObject, AttachmentFlags, "");
        spawnedItem.gameObject.SetActive(true);
        // TODO check if cleanup is possible when switching weapon
        spawnedItem.name = spawnedItem + hand.name;
        CleanHand(hand);
    }

    public void EmptyGunHand(Hand hand)
    {
        var emptyItem = Instantiate(EmptyHand);
        hand.AttachObject(emptyItem.gameObject, AttachmentFlags, "");
        emptyItem.gameObject.SetActive(true);
        emptyItem.name = emptyItem + hand.name;
        //CleanHand(hand);
    }


}
