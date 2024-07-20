using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DatingProfile : MonoBehaviour
{
    public static DatingProfile datingProfile;
    public int numRunes;
    public runes[] runeTypes;

    [SerializeField] Item[] items;

    // Start is called before the first frame update
    void Awake()
    {
        datingProfile = this;
    }

    private void Start()
    {
        GenerateProfile();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GenerateProfile()
    {
        numRunes = Random.Range(3, 6);
        runeTypes = new runes[numRunes];
        int numItems = items.Length;
        int randomIndex = Random.Range(0, numItems);
        switch (numRunes)
        {
            case 3:
                runeTypes[0] = items[randomIndex].runes[0];
                randomIndex = LoopIndex(randomIndex + Random.Range(0, numItems), numItems);
                runeTypes[1] = items[randomIndex].runes[0];
                randomIndex = LoopIndex(randomIndex + Random.Range(0, numItems), numItems);
                runeTypes[2] = items[randomIndex].runes[0];
                return;
            case 4:
                runeTypes[0] = items[randomIndex].runes[0];
                runeTypes[1] = items[randomIndex].runes[1];
                randomIndex = LoopIndex(randomIndex + Random.Range(0, numItems), numItems);
                runeTypes[2] = items[randomIndex].runes[0];
                runeTypes[3] = items[randomIndex].runes[1];
                return;
            case 5:
                runeTypes[0] = items[randomIndex].runes[0];
                runeTypes[1] = items[randomIndex].runes[1];
                runeTypes[2] = items[randomIndex].runes[2];
                runeTypes[3] = items[randomIndex].runes[3];
                runeTypes[4] = items[randomIndex].runes[4];
                return;
        }
    }

    private int LoopIndex(int index, int max)
    {
        return(index % max);
    }
}
