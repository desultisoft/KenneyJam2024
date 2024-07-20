using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConductingSpot : Interactable
{
    SummoningProgress ritualCircle;
    public RitualNode[] ritualNodes;
    [SerializeField] Vector3 positionOffset;

    public float delay;
    private bool conducting = false;
    private bool ritualStarted = false;
    private float ritualDelayTime = 0;
    private HubPlayer player;
    public override void Interact(GameObject interactor)
    {
        player = interactor.GetComponent<HubPlayer>();
        player.SetCanMove(false);
        player.SetAutoTarget(transform.position + positionOffset);
        ritualDelayTime = Time.realtimeSinceStartup + delay;
        conducting = true;
    }

    private void Start()
    {
        ritualCircle = (SummoningProgress)FindObjectOfType(typeof(SummoningProgress));
        ritualNodes = ritualCircle.GetComponentsInChildren<RitualNode>();
    }

    public void Update()
    {
        if (!conducting) return;
        if (!ritualStarted & ritualDelayTime <= Time.realtimeSinceStartup)
        {
            List<RitualNode> activeNodes = new List<RitualNode>();
            int numNodes = ritualNodes.Length;
            for (int i = 0; i < numNodes; i++)
            {
                if (ritualNodes[i].requiredRune != runes.none) activeNodes.Add(ritualNodes[i]);
            }
            // DEBUG: Simulate some nodes if we don't have any
            if (activeNodes.Count == 0)
            {
                activeNodes.Add(ritualNodes[0]);
                activeNodes.Add(ritualNodes[2]);
                activeNodes.Add(ritualNodes[4]);
            }
            ritualCircle.StartRitual(activeNodes,this);
            ritualStarted = true;
        }
    }

    public void DisconnectPlayer(float delay)
    {
        conducting = false;
        ritualStarted = false;
        IEnumerator coroutine = ReleasePlayer(delay);
        StartCoroutine(coroutine);
    }

    IEnumerator ReleasePlayer(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        player.SetCanMove(true);
    }
}
