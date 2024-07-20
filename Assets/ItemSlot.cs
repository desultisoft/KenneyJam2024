using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSlot : MonoBehaviour
{
    public string acceptedItem;

    public bool AcceptsItem(Item targetItem)
    {
        if (!targetItem) return false; 
        return targetItem.itemName == acceptedItem;
    }
}
