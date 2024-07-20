using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// If I draw a pentagram it unlocks a center.
/// Triangle gives you three spots.
/// Square gives you four.
/// When something is made incorrectly candles pop off, destroying lines.
/// </summary>


public class Pentagram : MonoBehaviour
{
    [Header("Data")]
    public List<int> activatedCandles = new List<int>();
    public List<Vector2Int> totalRequiredConnections = new List<Vector2Int>();
    public List<Vector2Int> currentRequiredConnections = new List<Vector2Int>();

    [Header("Slots")]
    public List<ItemSlot> candleSlots;


    int lastIndex = 0;

    PostProcessingController postProcessingController;
    ItemSacrifice itemSacrifice;

    private void Start()
    {
        postProcessingController = PostProcessingController.PostProcessingSingleton;
    }

    void Awake()
    {
        itemSacrifice = GetComponentInChildren<ItemSacrifice>();
        foreach(ItemSlot slot in candleSlots)
        {
            slot.onDeposit += HandleDeposit;
            slot.locked = true;
        }
    }

    private bool IsPentagramComplete()
    {
        if (activatedCandles.Count < totalRequiredConnections.Count)
        {
            return false;
        }
        currentRequiredConnections = new List<Vector2Int>(totalRequiredConnections);
        for (int i = currentRequiredConnections.Count - 1; i >= 0; i--)
        {
            Vector2Int requiredConnection = totalRequiredConnections[i];
            if (activatedCandles.Contains(requiredConnection.x) && activatedCandles.Contains(requiredConnection.y))
            {
                currentRequiredConnections.Remove(requiredConnection);
            }
        }

        if (currentRequiredConnections.Count == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void DisconnectAllCandles()
    {
        foreach(ItemSlot slot in candleSlots)
        {
            slot.Reject();
        }
        activatedCandles.Clear();
    }

    private void HandleDeposit(ItemSlot slot, Item item)
    {
        int depositIndex = candleSlots.FindIndex(x => x==slot);
        Vector2Int newConnection = new Vector2Int(lastIndex, depositIndex);
        bool isRequired = totalRequiredConnections.Contains(newConnection);
        Debug.Log($"Handling Deposit at Index {depositIndex}: {slot.gameObject.name}");
        Debug.Log($"Attempting connection: { newConnection } and requirement is: {isRequired} {activatedCandles.Count>0}");

        if (!isRequired && activatedCandles.Count > 0)
        {
            Debug.Log("Rejecting!");
            DisconnectAllCandles();
        }
        else
        {
            Debug.Log("Accepting");

            //Try to turn on the associated animation between the two spots on the pentagram.
            if (!activatedCandles.Contains(depositIndex))
                activatedCandles.Add(depositIndex);
        }

        lastIndex = depositIndex;

        if (IsPentagramComplete())
        {
            postProcessingController.StartChromaticEffect(0.25f, 0.75f);
            postProcessingController.StartCameraShake(0.05f, 0.2f);
            itemSacrifice.SetSacrificeSlots(totalRequiredConnections.Count());
        }
    }


    //Draw the current pentagram.
    public void OnDrawGizmosSelected()
    {
        List<Vector2Int> currentConnections = totalRequiredConnections.Except(currentRequiredConnections).ToList();
        float radius = 0.2f;
        foreach (Vector2Int connection in currentConnections)
        {

            ItemSlot slot1 = candleSlots[connection.x];
            ItemSlot slot2 = candleSlots[connection.y];

            Gizmos.DrawLine(slot1.transform.position, slot2.transform.position);

            Gizmos.DrawSphere(slot1.transform.position, radius);
            radius += 0.1f;
        }
    }
}
