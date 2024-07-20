using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : Interactable
{
    public int itemID;
    public runes[] runes;
    Collider2D coll;

    public GameObject GetGameObject() { return gameObject; }

    public void Awake()
    {
        coll = GetComponent<Collider2D>();
    }

    public override void Interact(GameObject interactor)
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
