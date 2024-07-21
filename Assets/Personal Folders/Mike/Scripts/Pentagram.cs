using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Pentagram : MonoBehaviour
{
    public AudioSource FireLine;
    public AudioSource FireLineStart;
    public static Pentagram pentagram;

    [Header("Data")]
    public List<int> activatedCandles = new List<int>();
    public List<Vector2Int> currentRequiredConnections = new List<Vector2Int>();
    public List<Vector2Int> currentConnections;

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

    public List<Animator> lineAnimators;

    private void Start()
    {
        postProcessingController = PostProcessingController.PostProcessingSingleton;
        datingProfile = DatingProfile.datingProfile;
        pentagram = this;
    }

    void Awake()
    {
        itemSacrifice = GetComponentInChildren<ItemSacrifice>(true);
        foreach(ItemSlot slot in candleSlots)
        {
            slot.onDeposit += HandleDeposit;
            slot.locked = true;
        }
    }

    private void Update()
    {
        UpdateLines();
        if (!requiredShape)
        {
            foreach(ItemSlot i in candleSlots)
            {
                i.gameObject.SetActive(false);
            }
        }
        else
        {
            foreach (ItemSlot i in candleSlots)
            {
                i.gameObject.SetActive(true);
            }
            currentConnections = requiredShape.totalRequiredConnections.Except(currentRequiredConnections).ToList();
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
            if (activatedCandles.Count == 0)
            {
                FireLineStart.Play();
            }    
            else if (activatedCandles.Count > 0)
            {
                FireLine.Play();
            }
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
        if (requiredShape == null)
            return;



        currentConnections = requiredShape.totalRequiredConnections.Except(currentRequiredConnections).ToList();
        //float radius = 0.2f;
        foreach (Vector2Int connection in currentConnections)
        {
            ItemSlot slot1 = candleSlots[connection.x];
            ItemSlot slot2 = candleSlots[connection.y];

            Gizmos.DrawLine(slot1.transform.position, slot2.transform.position);

            //Gizmos.DrawSphere(slot1.transform.position, radius);
            //radius += 0.1f;
        }

        
    }

    public bool CheckForContainsConnection(int index1, int index2)
    {
        return currentConnections.Contains(new Vector2Int(index1, index2)) || currentConnections.Contains(new Vector2Int(index2, index1));
    }

    void UpdateLines()
    {
        if (lineAnimators.Count == 0) return;
        //Activate line 1 
        if (CheckForContainsConnection(1,2))
        {
            lineAnimators[0].SetBool("Activate", true);
        }
        else
        {
            lineAnimators[0].SetBool("Activate", false);
        }

        //Activate line 2 
        if (CheckForContainsConnection(2, 0))
        {
            
            lineAnimators[1].SetBool("Activate", true);
        }
        else
        {
            lineAnimators[1].SetBool("Activate", false);
        }

        //Activate line 3
        if (CheckForContainsConnection(3, 4))
        {
            
            lineAnimators[2].SetBool("Activate", true);
        }
        else
        {
            lineAnimators[2].SetBool("Activate", false);
        }

        //Activate line 4
        if (CheckForContainsConnection(1, 0))
        {
            
            lineAnimators[3].SetBool("Activate", true);
        }
        else
        {
            lineAnimators[3].SetBool("Activate", false);
        }

        //Activate line 5
        if (CheckForContainsConnection(1, 3))
        {
            
            lineAnimators[4].SetBool("Activate", true);
        }
        else
        {
            lineAnimators[4].SetBool("Activate", false);
        }


        //Activate line 6
        if (CheckForContainsConnection(4, 0))
        {
            
            lineAnimators[5].SetBool("Activate", true);
        }
        else
        {
            lineAnimators[5].SetBool("Activate", false);
        }

        //Activate line 7
        if (CheckForContainsConnection(4, 1))
        {
            
            lineAnimators[6].SetBool("Activate", true);
        }
        else
        {
            lineAnimators[6].SetBool("Activate", false);
        }

        //Activate line 8
        if (CheckForContainsConnection(3,0))
        {
            
            lineAnimators[7].SetBool("Activate", true);
        }
        else
        {
            lineAnimators[7].SetBool("Activate", false);
        }
    }

}
