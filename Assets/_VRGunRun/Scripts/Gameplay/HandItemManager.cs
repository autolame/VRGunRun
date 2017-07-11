using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class HandItemManager : MonoBehaviour
{
    private Hand hand;

    public EmptyHand EmptyHand;
    public List<Gun> GunList = new List<Gun>();
    public int ActiveItemIndex;

    public List<GameObject> cleanUpListHand = new List<GameObject>();

    public Hand.AttachmentFlags AttachmentFlags;

    private void Awake()
    {
        hand = GetComponent<Hand>();
    }
    public void IncreaseItemIndex()
    {
        if (ActiveItemIndex < GunList.Count - 1)
        { ActiveItemIndex++; }
        else
        { ActiveItemIndex = 0; }
    }
    public void DecreaseItemIndex()
    {
        if (ActiveItemIndex > 0)
        { ActiveItemIndex--; }
        else
        { ActiveItemIndex = GunList.Count - 1; }
    }

    public void QueueForCleanUp(GameObject gameObject)
    {
        cleanUpListHand.Add(gameObject);
    }

    public void CleanHand()
    {
        foreach (var go in cleanUpListHand)
        {
            Destroy(go);
        }

        cleanUpListHand.Clear();
    }

    public void SpawnItemAndAttachToHand()
    {
        var spawnedItem = Instantiate(GunList[ActiveItemIndex]);
        hand.AttachObject(spawnedItem.gameObject, AttachmentFlags, "");
        spawnedItem.gameObject.SetActive(true);

        Debug.Log(hand.name + " : " + spawnedItem.name);
    }

    public void EmptyGunHand()
    {
        var emptyItem = Instantiate(EmptyHand);
        hand.AttachObject(emptyItem.gameObject, AttachmentFlags, "");
        emptyItem.gameObject.SetActive(true);

        Debug.Log(hand.name + " : empty hand");

        CleanHand();
    }


}
