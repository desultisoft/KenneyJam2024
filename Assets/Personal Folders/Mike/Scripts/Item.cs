using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public int itemID;

    Collider2D coll;

    public void Awake()
    {
        coll = GetComponent<Collider2D>();
    }

    public void Interact(GameObject interactor)
    {
        HubPlayerInteract target = interactor.GetComponent<HubPlayerInteract>();
        if (target == null)
            return;

        target.Carry(this);
    }

    public void EnablePickup(bool isOn)
    {
        coll.enabled = isOn;
    }
}
