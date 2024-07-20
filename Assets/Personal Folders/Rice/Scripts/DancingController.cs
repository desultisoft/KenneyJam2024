using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DancingController : MonoBehaviour
{
    int[] chants = new int[9];
    SummoningProgress summoningProgress;
    // Start is called before the first frame update
    void Start()
    {
        summoningProgress = GetComponent<SummoningProgress>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
