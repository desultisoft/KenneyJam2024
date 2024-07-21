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
    public float jumpPower = 2f;
    public int numJumps = 1;
    public float duration = 1f;
    public Material highlightMaterial;
    public Material defaultMaterial;
    public RandomSoundPlayer Grab;
    public RandomSoundPlayer GrabCandle;
    public RandomSoundPlayer GrabSkull;
    public RandomSoundPlayer PlaceSkull;
    public RandomSoundPlayer PlaceCandle;
    #region Getters/Setters
    private Interactable targetItem;
    public Interactable TargetItem
    {
        get
        {
            return targetItem;
        }
        set
        {
            if (targetItem == value)
            {
                return;
            }

            if(targetItem != null)
                TryHighLight(targetItem.gameObject, false);

            targetItem = value;

            if(targetItem != null)
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
                return;
            }

            if (targetSlot)
                TryHighLight(targetSlot.gameObject, false);

            targetSlot = value;

            if (targetSlot)
                TryHighLight(targetSlot.gameObject, true);
        }
    }

    private Item carriedItem;
    #endregion

    public void Awake()
    {
        hubPlayerDetect = GetComponent<HubPlayerDetect>();
        hubPlayer = GetComponent<HubPlayer>();
    }

    public void TryHighLight(GameObject target, bool isHighlighted)
    {
        //If we are not highlighting anything.
        if (target == null) return;

        if (isHighlighted)
        {
            //Grab the current renderer and save it's material.
            SpriteRenderer itemRenderer = target.gameObject.GetComponent<SpriteRenderer>();

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
                itemRenderer.material = defaultMaterial;
            }
        }
    }

    public void Carry(Item item)
    {
        if (!item)
            return;

        if (carriedItem)
            return;

        if (hubPlayerDetect.targetSlot && !hubPlayerDetect.targetSlot.locked && hubPlayerDetect.targetSlot.currentItem)
        {
            Debug.Log("Withdrawing!");
            carriedItem = hubPlayerDetect.targetSlot.Withdraw();
        }
        else
        {
            Debug.Log("Picking up!");
            carriedItem = item;
            Grab.PlayRandomSound();
            if (item.itemID == 0)
            {
                GrabCandle.PlayRandomSound();
            }
            else if (item.itemID == 1)
            {
                GrabSkull.PlayRandomSound();
            }
        }

        hubPlayer.SetCanMove(false);
        item.gameObject.transform.SetParent(pickupPoint);
        item.gameObject.transform.DOJump(pickupPoint.position, jumpPower, numJumps, duration)
            .SetEase(Ease.OutQuad)
            .OnComplete(() => hubPlayer.SetCanMove(true));
    }

    public void PutDown()
    {
        Debug.Log("putting Down item");

        Transform target = targetSlot ? targetSlot.transform : transform;

        if (targetSlot)
        {
            targetItem = null;
            targetSlot.DepositItem(carriedItem);
        }

        hubPlayer.SetCanMove(false);
        carriedItem.gameObject.transform.SetParent(null);
        carriedItem.gameObject.transform.DOJump(target.position, jumpPower, numJumps, duration)
            .SetEase(Ease.OutQuad)
            .OnComplete(() => {
                if (carriedItem.itemID == 0)
                {
                    PlaceCandle.PlayRandomSound();
                }
                else if (carriedItem.itemID == 1)
                {
                    PlaceSkull.PlayRandomSound();
                }
                hubPlayer.SetCanMove(true);
                carriedItem = null;

            });
    }

    public void CheckForTargets()
    {
        if (!carriedItem)
        {
            TargetItem = hubPlayerDetect.targetItem;
            TargetSlot = null;
        }
        else
        {
            TargetItem = null;
            if (hubPlayerDetect.targetSlot && hubPlayerDetect.targetSlot.AcceptsItem(carriedItem))
            {
                TargetSlot = hubPlayerDetect.targetSlot;
            }
            else
            {
                TargetSlot = null;
            }
        }
    }

    private void Update()
    {
        CheckForTargets();

        if (Input.GetKeyDown(KeyCode.Space) && hubPlayer.canMove)
        {
            if (carriedItem)
            {
                PutDown();
            }
            else if(targetItem != null)
            {
                targetItem.Interact(gameObject);
            }
        }
    }
}
