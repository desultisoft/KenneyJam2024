using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSacrifice : MonoBehaviour
{
    private ItemSlot[] itemSlots;
    private DatingProfile datingProfile;
    private PostProcessingController postProcessingController;
    private int activeSlots;
    private void Awake()
    {
        itemSlots = GetComponentsInChildren<ItemSlot>();
        int numSlots = itemSlots.Length;
        foreach (ItemSlot itemSlot in itemSlots) itemSlot.gameObject.SetActive(false);
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
        switch (numCandles)
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

    bool CompareRunes()
    {
        runes[] correctRunes = datingProfile.runeTypes;
        runes[] itemRunes = new runes[correctRunes.Length];
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
            itemRunes[3] = itemSlots[0].currentItem.runes[4];
        }
        for (int i = 0; i < correctRunes.Length; i++)
        {
            if (itemRunes[i] != correctRunes[i]) return false;
        }
        postProcessingController.StartChromaticEffect(0.25f, 0.75f);
        postProcessingController.StartCameraShake(0.05f, 0.2f);
        return true;
    }
}
