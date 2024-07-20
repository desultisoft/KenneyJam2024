using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HubPlayerInteract : MonoBehaviour
{
    [SerializeField] private HubPlayerDetect hubPlayerDetect;
    [SerializeField] private HubPlayer hubPlayer;
    

    public Transform pickupPoint;
    public Item carriedItem;
    private bool isCarrying;
    public float jumpPower = 2f;
    public int numJumps = 1;
    public float duration = 1f;
    private Transform dropPosition;

    public Material highlightMaterial;
    private Material previousItemMaterial;

    #region Getters/Setters
    private Item targetItem;
    public Item TargetItem
    {
        get
        {
            return targetItem;
        }
        set
        {
            if (targetItem == value)
            {
                Debug.Log("Not Changing target!");
                return;
            }

            if(targetItem)
                TryHighLight(targetItem.gameObject, false);

            targetItem = value;

            if(targetItem)
                TryHighLight(targetItem.gameObject, true);
        }
    }

    private ItemSlot targetSlot;
    public ItemSlot TargetSlot
    {
        get
        {
            return targetSlot;
        }
        set
        {
            if (targetSlot == value)
            {
                Debug.Log("Not Changing target!");
                return;
            }

            if (targetSlot)
                TryHighLight(targetItem.gameObject, false);

            targetSlot = value;

            if (targetSlot)
                TryHighLight(targetItem.gameObject, true);
        }
    }
    #endregion

    public void Awake()
    {
        hubPlayerDetect = GetComponent<HubPlayerDetect>();
        hubPlayer = GetComponent<HubPlayer>();
    }

    public void TryHighLight(GameObject target, bool isHighlighted)
    {
        Debug.Log($"Trying Highlight{target}, {isHighlighted}");

        //If we are not highlighting anything.
        if (target == null) return;

        if (isHighlighted)
        {
            //Grab the current renderer and save it's material.
            SpriteRenderer itemRenderer = target.gameObject.GetComponent<SpriteRenderer>();
            previousItemMaterial = itemRenderer.material;

            //Highlight it.
            if (itemRenderer != null)
            {
                itemRenderer.material = highlightMaterial;
            }
        }
        else
        {
            //We have stopped highlighting something, give it back it's saved material.
            SpriteRenderer itemRenderer = target.gameObject.GetComponent<SpriteRenderer>();
            //Stop Highlighting it.
            if (itemRenderer != null)
            {
                itemRenderer.material = previousItemMaterial;
            }
            previousItemMaterial = null;
        }
    }

    public void Carry(Item item)
    {
        isCarrying = true;
        carriedItem = item;
        hubPlayer.canMove = false;

        item.gameObject.transform.SetParent(pickupPoint);
        item.gameObject.transform.DOJump(pickupPoint.position, jumpPower, numJumps, duration)
            .SetEase(Ease.OutQuad)
            .OnComplete(() => hubPlayer.canMove = true);
    }

    public void PutDown()
    {
        Transform target = dropPosition ? dropPosition : transform;

        isCarrying = false;
        hubPlayer.canMove = false;

        carriedItem.gameObject.transform.SetParent(null);
        carriedItem.gameObject.transform.DOJump(target.position, jumpPower, numJumps, duration)
            .SetEase(Ease.OutQuad)
            .OnComplete(() => { hubPlayer.canMove = true; carriedItem = null; });
    }

    public void CheckForTargets()
    {
        if (!isCarrying)
        {
            TargetItem = hubPlayerDetect.targetItem;
        }
        else
        {
            TargetItem = null;
        }


        if (isCarrying && hubPlayerDetect.targetSlot && hubPlayerDetect.targetSlot.AcceptsItem(targetItem))
        {
            TargetSlot = hubPlayerDetect.targetSlot;
        }
    }

    private void Update()
    {
        CheckForTargets();

        if (Input.GetKeyDown(KeyCode.Space) && hubPlayer.canMove)
        {
            if (isCarrying)
            {
                Debug.Log("putting down!");
                PutDown();
            }
            else
            {
                targetItem.Interact(gameObject);
            }
        }
    }
}
