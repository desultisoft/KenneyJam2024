using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PentagramChecker : MonoBehaviour
{
    public List<int> activatedCandles = new List<int>();
    public List<Vector2Int> requiredConnectionsForPentagram = new List<Vector2Int>();
    public List<Vector2Int> neededConnections;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            CheckPentagram();
        }
        // Check for number key presses to activate candles
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            ActivateCandle(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ActivateCandle(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ActivateCandle(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ActivateCandle(3);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            ActivateCandle(4);
        }
    }

    public void ActivateCandle(int candleID)
    {
        if (!activatedCandles.Contains(candleID))
        {
            activatedCandles.Add(candleID);
            Debug.Log("Activated candle: " + candleID);
        }
    }

    public void CheckPentagram()
    {
        if (IsPentagramCorrect())
        {
            Debug.Log("Pentagram completed successfully!");
        }
        else
        {
            Debug.Log("Incorrect pentagram.");
        }
    }

    private bool IsPentagramCorrect()
    {
        if (activatedCandles.Count < requiredConnectionsForPentagram.Count)
        {
            return false;
        }
        neededConnections = new List<Vector2Int>(requiredConnectionsForPentagram);
        for (int i = neededConnections.Count-1; i >= 0; i--)
        {
            Vector2Int requiredConnection = requiredConnectionsForPentagram[i];
            if (activatedCandles.Contains(requiredConnection.x)&&activatedCandles.Contains(requiredConnection.y))
            {
                neededConnections.Remove(requiredConnection);
            }
        }

        if(neededConnections.Count == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
