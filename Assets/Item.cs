using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public string itemName;

    public void Interact(GameObject interactor)
    {
        HubPlayerInteract target = interactor.GetComponent<HubPlayerInteract>();
        if (target == null)
            return;
        target.Carry(this);
    }
}
