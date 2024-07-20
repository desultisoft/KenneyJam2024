using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSlot : MonoBehaviour
{
    public bool locked;
    public List<int> acceptedItemIDs;
    public Item currentItem;

    [Header("Order")]
    public int order;

    [Header("Rejection")]
    public float jumpPower = 2f;
    public int numJumps = 1;
    public float shakeDuration = 1f;
    public float jumpDuration = 0.2f;
    public float scatter = 0.2f;
    public float avoidance = 1f;
    public float shakeDelay = 0.3f;

    public event Action<ItemSlot, Item> onDeposit = delegate{ };
    public event Action<ItemSlot, Item> onWithdraw = delegate { };

    public bool AcceptsItem(Item targetItem)
    {
        if (!targetItem || currentItem)
            return false;

        return acceptedItemIDs.Contains(targetItem.itemID);
    }

    public bool DepositItem(Item depositedItem)
    {
        if (!depositedItem)
            return false;

        if (!AcceptsItem(depositedItem))
            return false;

        currentItem = depositedItem;

        if (locked)
            currentItem.EnablePickup(false);

        onDeposit.Invoke(this, depositedItem);

        return true;
    }

    public Item Withdraw()
    {
        Item withdrawn = currentItem;
        currentItem = null;
        onWithdraw.Invoke(this, withdrawn);
        return withdrawn;
    }

    public void Reject()
    {
        if (!currentItem)
            return;

        StartCoroutine(HandleRejection());

        
    }

    public IEnumerator HandleRejection()
    {
        Vector3 scatteramount = new Vector3(1, 1, 0) * UnityEngine.Random.Range(-scatter, scatter);
        Vector3 jumpTarget = transform.localPosition.normalized * avoidance;

        currentItem.EnablePickup(false);
        Sequence mySequence = DOTween.Sequence().Pause();
        mySequence.Append(currentItem.transform.DOShakePosition(shakeDuration, new Vector3(0.018f, 0, 0), 20, 0, false, false));
        mySequence.Append(currentItem.gameObject.transform.DOJump(jumpTarget + transform.position, jumpPower, numJumps, jumpDuration)
            .SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                currentItem.EnablePickup(true);
                currentItem = null;
            }
        ));

        yield return new WaitForSeconds(shakeDelay);

        mySequence.Play();
        
        
    }
}
