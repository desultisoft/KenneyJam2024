using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemSacrifice : MonoBehaviour
{
    private ItemSlot[] itemSlots;
    int filledSlots;
    public ItemSlot[] ItemSlots { get { return itemSlots; } }
    private DatingProfile datingProfile;
    private PostProcessingController postProcessingController;
    private int activeSlots;

    private GameObject conductingSpot;

    private void Awake()
    {
        itemSlots = GetComponentsInChildren<ItemSlot>();
        int numSlots = itemSlots.Length;
        foreach (ItemSlot itemSlot in itemSlots)
        {
            itemSlot.gameObject.SetActive(false);
            itemSlot.onDeposit += HandleDeposit;
            itemSlot.onWithdraw += HandleWithdraw;
        }
        conductingSpot = GameObject.Find("Conducting spot");
        conductingSpot.SetActive(false);
        if (numSlots < 3)
        {
            Debug.LogError("This system assumes at least 3 slots");
            Debug.Break();
        }
    }

    private void Start()
    {
        postProcessingController = PostProcessingController.PostProcessingSingleton;
        datingProfile = DatingProfile.datingProfile;
    }
    public void SetSacrificeSlots(int numCandles)
    {
        int numRunes = datingProfile.numRunes;
        switch (numRunes)
        {
            case 5:
                itemSlots[0].gameObject.SetActive(true);
                activeSlots = 1;
                return;
            case 4:
                itemSlots[0].gameObject.SetActive(true);
                itemSlots[0].transform.position = new Vector3(-0.75f, 0);
                itemSlots[1].gameObject.SetActive(true);
                itemSlots[1].transform.position = new Vector3(0.75f, 0);
                activeSlots = 2;
                return;
            case 3:
                itemSlots[0].gameObject.SetActive(true);
                itemSlots[0].transform.position = new Vector3(-0.375f, 0.375f); 
                itemSlots[1].gameObject.SetActive(true);
                itemSlots[1].transform.position = new Vector3(0, -0.375f);
                itemSlots[2].gameObject.SetActive(true);
                itemSlots[2].transform.position = new Vector3(0.375f, 0.375f);
                activeSlots = 3;
                return;
        }
    }

    public void ResetRitual()
    {
        foreach(ItemSlot itemSlot in itemSlots)
        {
            itemSlot.gameObject.SetActive(false);
            itemSlot.transform.position = Vector3.zero;
        }
    }

    void HandleWithdraw(ItemSlot slot, Item item)
    {
        filledSlots--;
    }

    void HandleDeposit(ItemSlot slot, Item item)
    {
        filledSlots++;
        if (filledSlots >= activeSlots)
        {
            if (CompareRunes())
            {
                conductingSpot.SetActive(true);
                postProcessingController.StartChromaticEffect(0.4f, 1.0f);
                postProcessingController.StartCameraShake(0.1f, 0.4f);
            }
        }
    }

    bool CompareRunes()
    {
        runes[] correctRunes = datingProfile.runeTypes;
        List<runes> itemRunes = GetRunes();
        print(itemRunes.Count());
        if (activeSlots == 3)
        {
            itemRunes[0] = itemSlots[0].currentItem.runes[0];
            itemRunes[1] = itemSlots[1].currentItem.runes[0];
            itemRunes[2] = itemSlots[2].currentItem.runes[0];
        }
        else if (activeSlots == 2)
        {
            itemRunes[0] = itemSlots[0].currentItem.runes[0];
            itemRunes[1] = itemSlots[0].currentItem.runes[1];
            itemRunes[2] = itemSlots[1].currentItem.runes[0];
            itemRunes[3] = itemSlots[1].currentItem.runes[1];
        }
        else if (activeSlots == 1)
        {
            itemRunes[0] = itemSlots[0].currentItem.runes[0];
            itemRunes[1] = itemSlots[0].currentItem.runes[1];
            itemRunes[2] = itemSlots[0].currentItem.runes[2];
            itemRunes[3] = itemSlots[0].currentItem.runes[3];
            itemRunes[4] = itemSlots[0].currentItem.runes[4];
        }
        for (int i = 0; i < correctRunes.Length; i++)
        {
            print("Expected rune " + correctRunes[i] + " Got rune " + itemRunes[i]);
            if (itemRunes[i] != correctRunes[i]) return false;
        }
        return true;
    }


    public List<runes> GetRunes()
    {
        if (itemSlots.Length == 0) return new List<runes>();

        IEnumerable<Item> items = ItemSlots.Where(x => x.gameObject.activeInHierarchy).Select(x => x.currentItem);
        List<runes> allRunes = items.SelectMany(item => item.runes).ToList();
        return allRunes;
    }
}
