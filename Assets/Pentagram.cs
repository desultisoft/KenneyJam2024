using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// If I draw a pentagram it unlocks a center.
/// Triangle gives you three spots.
/// Square gives you four.
/// When something is made incorrectly candles pop off, destroying lines.
/// </summary>

public class Pentagram : MonoBehaviour
{
    public List<ItemSlot> candleSlots;
    public List<ItemSlot> ingredientSlots;
    private LineRenderer lineRenderer;

    void Start()
    {
        //pentagramSpots = new List<Slot>();
       // lineRenderer = GetComponent<LineRenderer>();
        //if (pentagramSpots.Count < 2)
        //{
         //   Debug.LogWarning("At least two points are needed for pentagram.");
        //    return;
       // }
    }

    public void Update()
    {
        
    }
}
