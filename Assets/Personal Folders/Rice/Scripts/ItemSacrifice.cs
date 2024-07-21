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

    private RitualNode[] groundRunes;

    private GameObject conductingSpot;

    private void Awake()
    {
        itemSlots = GetComponentsInChildren<ItemSlot>(true);
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

        groundRunes = GetComponentsInChildren<RitualNode>();
        foreach (RitualNode node in groundRunes)
            node.gameObject.SetActive(false);
    }

    private void Start()
    {
        postProcessingController = PostProcessingController.PostProcessingSingleton;
        datingProfile = DatingProfile.datingProfile;
    }

    public void SetSacrificeSlots(int numCandles)
    {
        if (!datingProfile)
        {
            Debug.Log("No Dating Profile for sacrifice!");
            return;
        }

        int numRunes = datingProfile.numRunes;
        switch (numRunes)
        {
            case 5:
                itemSlots[0].gameObject.SetActive(true);
                itemSlots[0].transform.position = new Vector3(0.0f, 0.3125f);
                activeSlots = 1;
                return;
            case 4:
                itemSlots[0].gameObject.SetActive(true);
                itemSlots[0].transform.position = new Vector3(-0.625f, -0.125f);
                itemSlots[1].gameObject.SetActive(true);
                itemSlots[1].transform.position = new Vector3(0.625f, -0.125f);
                activeSlots = 2;
                return;
            case 3:
                itemSlots[0].gameObject.SetActive(true);
                itemSlots[0].transform.position = new Vector3(0, 0.3f); 
                itemSlots[1].gameObject.SetActive(true);
                itemSlots[1].transform.position = new Vector3(0.5f, -0.64f);
                itemSlots[2].gameObject.SetActive(true);
                itemSlots[2].transform.position = new Vector3(-0.5f, -0.64f);
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

    void HandleWithdraw(ItemSlot itemSlot, Item item)
    {
        filledSlots--;
        int slot = itemSlot.order;
        switch (activeSlots)
        {
            case 1:
                groundRunes[0].gameObject.SetActive(false);
                groundRunes[0].transform.position = new Vector3(0.8f, 1.35f);
                groundRunes[0].ChangeRune(runes.none, false);

                groundRunes[1].gameObject.SetActive(false);
                groundRunes[1].transform.position = new Vector3(1.6f, -0.175f);
                groundRunes[1].ChangeRune(runes.none, false);

                groundRunes[2].gameObject.SetActive(false);
                groundRunes[2].transform.position = new Vector3(0.0f, -1.65f);
                groundRunes[2].ChangeRune(runes.none, false);

                groundRunes[3].gameObject.SetActive(false);
                groundRunes[3].transform.position = new Vector3(-1.6f, -0.175f);
                groundRunes[3].ChangeRune(runes.none, false);

                groundRunes[4].gameObject.SetActive(false);
                groundRunes[4].transform.position = new Vector3(-0.8f, 1.35f);
                groundRunes[4].ChangeRune(runes.none, false);
                break;
            case 2:
                Vector3[] positions = { new Vector3(0.0f, 1.35f), new Vector3(1.75f, -0.175f), new Vector3(0.0f, -1.65f), new Vector3(-1.75f, -0.175f) };
                groundRunes[slot * 2].gameObject.SetActive(false);
                groundRunes[slot * 2].transform.position = positions[slot * 2];
                groundRunes[slot * 2].ChangeRune(runes.none, false);

                groundRunes[slot * 2 + 1].gameObject.SetActive(true);
                groundRunes[slot * 2 + 1].transform.position = positions[slot * 2 + 1];
                groundRunes[slot * 2 + 1].ChangeRune(runes.none, false);
                break;
            case 3:
                Vector3[] positions2 = { new Vector3(1.2f, 0.65f), new Vector3(0.0f, -1.65f), new Vector3(-1.2f, 0.65f) };
                groundRunes[slot].gameObject.SetActive(true);
                groundRunes[slot].transform.position = positions2[slot];
                groundRunes[slot].ChangeRune(runes.none, false);
                break;
        }
    }

    void HandleDeposit(ItemSlot slot, Item item)
    {
        filledSlots++;
        DrawRunes(item, slot.order);
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

    void DrawRunes(Item item, int slot)
    {
        bool itemCorrect = false;
        switch (activeSlots)
        {
            case 1:
                itemCorrect = CompareRune(item, slot);
                groundRunes[0].gameObject.SetActive(true);
                groundRunes[0].transform.position = new Vector3(0.8f, 1.35f);
                groundRunes[0].ChangeRune(item.runes[0], itemCorrect);

                groundRunes[1].gameObject.SetActive(true);
                groundRunes[1].transform.position = new Vector3(1.6f, -0.175f);
                groundRunes[1].ChangeRune(item.runes[1], itemCorrect);

                groundRunes[2].gameObject.SetActive(true);
                groundRunes[2].transform.position = new Vector3(0.0f, -1.65f);
                groundRunes[2].ChangeRune(item.runes[2], itemCorrect);

                groundRunes[3].gameObject.SetActive(true);
                groundRunes[3].transform.position = new Vector3(-1.6f, -0.175f);
                groundRunes[3].ChangeRune(item.runes[3], itemCorrect);

                groundRunes[4].gameObject.SetActive(true);
                groundRunes[4].transform.position = new Vector3(-0.8f, 1.35f);
                groundRunes[4].ChangeRune(item.runes[4], itemCorrect);
                break;
            case 2:
                Vector3[] positions = { new Vector3(0.0f, 1.35f), new Vector3(1.75f, -0.175f), new Vector3(0.0f, -1.65f), new Vector3(-1.75f, -0.175f) };
                itemCorrect = CompareRune(item, slot);
                print("Slot: " + slot + "runes: " + (int)item.runes[0] + " " + (int)item.runes[1]);
                groundRunes[slot * 2].gameObject.SetActive(true);
                groundRunes[slot * 2].transform.position = positions[slot * 2];
                groundRunes[slot * 2].ChangeRune(item.runes[0], itemCorrect);

                groundRunes[slot * 2 + 1].gameObject.SetActive(true);
                groundRunes[slot * 2 + 1].transform.position = positions[slot * 2 + 1];
                groundRunes[slot * 2 + 1].ChangeRune(item.runes[1], itemCorrect);
                break;
            case 3:
                Vector3[] positions2 = { new Vector3(1.2f, 0.65f), new Vector3(0.0f, -1.65f), new Vector3(-1.2f, 0.65f) };
                itemCorrect = CompareRune(item, slot);
                groundRunes[slot].gameObject.SetActive(true);
                groundRunes[slot].transform.position = positions2[slot];
                groundRunes[slot].ChangeRune(item.runes[0], itemCorrect);
                break;
        }
    }

    bool CompareRune(Item item, int slot)
    {
        runes[] correctRunes = datingProfile.runeTypes;
        runes[] itemRunes = item.runes;
        if (activeSlots == 3)
        {
            if (itemRunes[0] != correctRunes[slot]) return false;
            return true;
        }
        else if (activeSlots == 2)
        {
            for (int i = 0; i < 2; i++)
            {
                if (itemRunes[i] != correctRunes[i + slot*2]) return false;
            }
            return true;
        }
        else if (activeSlots == 1)
        {
            for (int i = 0; i < correctRunes.Length; i++)
            {
                if (itemRunes[i] != correctRunes[i]) return false;
            }
            return true;
        }
        return false;
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
