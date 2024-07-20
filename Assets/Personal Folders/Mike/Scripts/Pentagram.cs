using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Pentagram : MonoBehaviour
{
    public static Pentagram pentagram;
    [Header("Data")]
    public List<int> activatedCandles = new List<int>();
    public List<Vector2Int> currentRequiredConnections = new List<Vector2Int>();
    public Shape requiredShape;

    [Header("Slots")]
    public List<ItemSlot> candleSlots;

    [Header("Debugging Shapes")]
    [SerializeField] Shape pentagramShape;
    [SerializeField] Shape squareShape;
    [SerializeField] Shape triangleShape;

    int lastIndex = 0;

    PostProcessingController postProcessingController;
    DatingProfile datingProfile;
    ItemSacrifice itemSacrifice;

    private void Start()
    {
        postProcessingController = PostProcessingController.PostProcessingSingleton;
        datingProfile = DatingProfile.datingProfile;
        pentagram = this;
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

    public void SetShape(int _numRunes)
    {
        switch (_numRunes)
        {
            case 3:
                requiredShape = triangleShape;
                break;
            case 4:
                requiredShape = squareShape;
                break;
            case 5:
                requiredShape = pentagramShape;
                break;
        }
    }

    private bool IsPentagramComplete()
    {
        currentRequiredConnections = new List<Vector2Int>(requiredShape.totalRequiredConnections);
        for (int i = currentRequiredConnections.Count - 1; i >= 0; i--)
        {
            Vector2Int requiredConnection = requiredShape.totalRequiredConnections[i];
            if (activatedCandles.Contains(requiredConnection.x) && activatedCandles.Contains(requiredConnection.y))
            {
                currentRequiredConnections.Remove(requiredConnection);
            }
        }

        if (activatedCandles.Count < requiredShape.totalRequiredConnections.Count)
        {
            return false;
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
        Vector2Int reverseConnection = new Vector2Int(depositIndex, lastIndex);

        bool isRequired = requiredShape.totalRequiredConnections.Contains(newConnection) || requiredShape.totalRequiredConnections.Contains(reverseConnection);

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
            itemSacrifice.SetSacrificeSlots(requiredShape.totalRequiredConnections.Count());
        }
    }

    //Draw the current pentagram.
    public void OnDrawGizmosSelected()
    {
        if (requiredShape == null) return;

        List<Vector2Int> currentConnections = requiredShape.totalRequiredConnections.Except(currentRequiredConnections).ToList();
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
