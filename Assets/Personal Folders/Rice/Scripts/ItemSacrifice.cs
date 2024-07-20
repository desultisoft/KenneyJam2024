using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSacrifice : MonoBehaviour
{
    Transform[] itemSlots;
    private void Awake()
    {
        int numSlots = transform.childCount;
        itemSlots = new Transform[numSlots];
        for (int i = 0; i < numSlots; i++)
        {
            itemSlots[i] = transform.GetChild(i);
            itemSlots[i].gameObject.SetActive(false);
        }
        if (numSlots < 3)
        {
            Debug.LogError("This system assumes at least 3 slots");
            Debug.Break();
        }
    }
    public void SetSacrificeSlots(int numCandles)
    {
        switch (numCandles)
        {
            case 5:
                itemSlots[0].gameObject.SetActive(true);
                return;
            case 4:
                itemSlots[0].gameObject.SetActive(true);
                itemSlots[0].position = new Vector3(-0.75f, 0);
                itemSlots[1].gameObject.SetActive(true);
                itemSlots[1].position = new Vector3(0.75f, 0);
                return;
            case 3:
                itemSlots[0].gameObject.SetActive(true);
                itemSlots[0].position = new Vector3(-0.375f, 0.375f); 
                itemSlots[1].gameObject.SetActive(true);
                itemSlots[1].position = new Vector3(0, -0.375f);
                itemSlots[1].gameObject.SetActive(true);
                itemSlots[1].position = new Vector3(0.375f, 0.375f);
                return;
        }
    }

    public void ResetRitual()
    {
        int numSlots = itemSlots.Length;
        for (int i = 0; i < numSlots; i++)
        {
            itemSlots[i] = transform.GetChild(i);
            itemSlots[i].gameObject.SetActive(false);
        }
    }
}
