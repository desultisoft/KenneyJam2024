using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummoningProgress : MonoBehaviour
{
    [SerializeField] float fillProgressRate;
    [SerializeField] float failFlashRate;
    [SerializeField] float[] progressIntervals;
    private List<RitualNode> ritualNodes;
    private SpriteRenderer progressRenderer;
    private PostProcessingController postProcessingController;
    private ParticleSystem runeParticleSystem;
    private int arcPointID;

    private float numSymbols;
    public float progress = 0.0f;
    private bool progressStarted = false;

    private int lastChant = 0;

    [HideInInspector] public ConductingSpot conductingSpot;

    private void Start()
    {
        progressRenderer = transform.Find("CircleProgress").GetComponent<SpriteRenderer>();
        postProcessingController = PostProcessingController.PostProcessingSingleton;
        runeParticleSystem = GetComponentInChildren<ParticleSystem>();
        arcPointID = Shader.PropertyToID("_Arc2");
    }

    private void Update()
    {
        if (progress == 0) return;
        float renderProgress = 360.0f - progress * 360.0f;
        progressRenderer.material.SetFloat(arcPointID, renderProgress);
        if (progress > progressIntervals[lastChant])
        {
            if (ritualNodes[lastChant].chantSucceeded == false)
            {
                FailRitual();
                return;
            }
            lastChant++;
        }
        AttemptChant();
    }

    private void AttemptChant()
    {
        int chantIndex = lastChant;
        runes chantType = runes.none;
        if (Input.GetKeyDown(KeyCode.UpArrow)) chantType = runes.N;
        else if (Input.GetKeyDown(KeyCode.DownArrow)) chantType = runes.T;
        else if (Input.GetKeyDown(KeyCode.LeftArrow)) chantType = runes.L;
        else if (Input.GetKeyDown(KeyCode.RightArrow)) chantType = runes.I;

        if (chantType < 0) return;
        if (ritualNodes[chantIndex].chantAttempted == true)
        {
            FailRitual();
            return;
        }
        if (!ritualNodes[chantIndex].CompareRune(chantType)) FailRitual();
        else
        {
            postProcessingController.StartChromaticEffect(0.4f,0.3f);
            postProcessingController.StartCameraShake(0.05f, 0.3f);
            runeParticleSystem.emission.SetBurst(0, new ParticleSystem.Burst(0.0f, 1, (short)chantIndex, 2 * (chantIndex + 1), 0.035f));
            runeParticleSystem.Play();
        }
    }

    private void FixedUpdate()
    {
        if (progressStarted == false) return;
        progress += fillProgressRate;
        if (progress >= 1.0f)
        {
            CheckRitual();
        }
    }

    public void StartRitual(List<RitualNode> _ritualNodes, ConductingSpot _conductingSpot)
    {
        conductingSpot = _conductingSpot;
        ritualNodes = _ritualNodes;
        numSymbols = ritualNodes.Count;
        if (progressIntervals.Length != numSymbols)
        {
            Debug.LogError("Number of progress intervals incorrect");
            Debug.Break();
        }
        progress = 0;
        lastChant = 0;
        progressStarted = true;
    }

    public void CheckRitual()
    {
        bool succeeded = true;
        foreach (RitualNode node in ritualNodes)
        {
            if (node.chantSucceeded == false) succeeded = false; ;
        }
        if (succeeded) CompleteRitual();
        else FailRitual();
    }

    private bool CheckRitualStep(int step)
    {
        return ritualNodes[step].chantSucceeded;
    }

    void FailRitual()
    {
        progress = 0;
        lastChant = 0;
        progressStarted = false;
        conductingSpot.DisconnectPlayer(0.3f);
        foreach (RitualNode node in ritualNodes) node.ResetState();
        StartCoroutine("FailFlash");
    }

    private void CompleteRitual()
    {
        progress = 0;
        postProcessingController.StartChromaticEffect(1.0f,1.7f);
        postProcessingController.StartCameraShake(0.2f, 1.2f);
        runeParticleSystem.emission.SetBurst(0, new ParticleSystem.Burst(0.0f, 1, 1, 70, 0.035f));
        conductingSpot.DisconnectPlayer(1.3f);
        progressStarted = false;
    }

    private IEnumerator FailFlash()
    {
        float flashProgress = 0.0f;
        float flashDirection = 1.0f;
        Color origingColor = progressRenderer.color;
        while (flashProgress >= 0)
        {
            flashProgress += failFlashRate * flashDirection;
            progressRenderer.color = Vector4.Lerp(origingColor, Color.cyan, flashProgress);
            if (flashProgress > 1.0f)
            {
                flashProgress = 1.0f;
                origingColor.a = 0.0f;
                flashDirection = -1.0f;
            }
            yield return new WaitForFixedUpdate();
        }
        progressRenderer.material.SetFloat(arcPointID, 360);
        origingColor.a = 1.0f;
        progressRenderer.color = origingColor;
    }
}
