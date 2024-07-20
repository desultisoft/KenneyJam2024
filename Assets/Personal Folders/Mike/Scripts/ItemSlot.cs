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

    [Header("Rejection")]
    public float jumpPower = 2f;
    public int numJumps = 1;
    public float duration = 1f;
    public float scatter = 0.2f;

    [Header("Shake")]
    public float shakeDelay = 0.3f;
    public float shakeDuration = 0.25f;
    public float shakeStrength = 0.25f;
    public int shakeVibrato = 1;
    public float shakeRandomness = 0.25f;

    public event Action<ItemSlot, Item> onDeposit = delegate{ };
    public event Action<ItemSlot, Item> onWithdraw = delegate { };




    public bool AcceptsItem(Item targetItem)
    {
        if (!targetItem)
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
        currentItem.EnablePickup(false);

        yield return new WaitForSeconds(0.5f);
        Sequence sequence = DOTween.Sequence();
        sequence.Append(currentItem.transform.DOShakePosition(shakeDuration, shakeStrength, shakeVibrato, shakeRandomness));
        sequence.Append(currentItem.gameObject.transform.DOJump(transform.position + (new Vector3(1, 1, 0) * UnityEngine.Random.Range(-scatter, scatter)), jumpPower, numJumps, duration)
            .SetEase(Ease.OutQuad)
            .OnComplete(() => {
                currentItem.EnablePickup(true);
                currentItem = null;
            }));
    }
}
