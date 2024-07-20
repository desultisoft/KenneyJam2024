using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummoningProgress : MonoBehaviour
{
    [SerializeField] float progressRate;
    [SerializeField] HighlightFlash[] circleFlashes;
    private int numSymbols;
    private float progress = 0.0f;
    private bool progressStarted = false;

    private int[] chants;
    private bool[] chantsTriggered;
    private bool[] chantsAttempted;
    private bool[] chantsSucceeded;
    private int lastChant = 0;

    [SerializeField] SpriteRenderer[] symbols;
    [SerializeField] Sprite[] symbolSprites;


    private void Start()
    {
        numSymbols = circleFlashes.Length;
        chantsTriggered = new bool[numSymbols];
        chantsAttempted = new bool[numSymbols];
        chantsSucceeded = new bool[numSymbols];
        chants = new int[numSymbols];
}

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) GeneratePattern();
        if (Input.GetKeyDown(KeyCode.Return)) StartRitual();
        if (progress == 0) return;
        if (progress > (lastChant / (float)numSymbols))
        {
            chantsTriggered[lastChant] = true;
            circleFlashes[lastChant].StartFlash();
            lastChant++;
        }
        AttemptChant();
    }

    private void AttemptChant()
    {
        int chantIndex = lastChant - 1;
        if (chantsAttempted[chantIndex]) return;
        int chantType = -1;
        if (Input.GetKeyDown(KeyCode.UpArrow)) chantType = 0;
        else if (Input.GetKeyDown(KeyCode.DownArrow)) chantType = 1;
        else if (Input.GetKeyDown(KeyCode.LeftArrow)) chantType = 2;
        else if (Input.GetKeyDown(KeyCode.RightArrow)) chantType = 3;

        if (chantType < 0) return;
        chantsAttempted[chantIndex] = true;
        if (chantType == chants[chantIndex])
        {
            chantsSucceeded[chantIndex] = true;
            symbols[chantIndex].color = Color.green;
        }
        else CheckRitual();
    }

    private void FixedUpdate()
    {
        if (progressStarted == false) return;
        progress += progressRate;
        if (progress >= 1.0f)
        {
            progress = 0;
            lastChant = 0;
            progressStarted = false;
            CheckRitual();
        }
    }

    public void StartRitual()
    {
        progress = 0;
        lastChant = 0;
        progressStarted = true;
    }

    void GeneratePattern()
    {
        int numChants = chants.Length;
        for (int i = 0; i < numChants; i++)
        {
            chants[i] = Random.Range(0, 4);
            symbols[i].sprite = symbolSprites[chants[i]];
        }
    }

    public void CheckRitual()
    {
        bool succeeded = true;
        for (int i = 0; i < numSymbols; i++)
        {
            if (chantsSucceeded[i] == false) succeeded = false;
            chantsSucceeded[i] = false;
            chantsAttempted[i] = false;
            symbols[i].color = Color.white;
        }
        if (succeeded) CompleteRitual();
    }

    private void CompleteRitual()
    {

    }
}
