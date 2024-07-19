using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulseBreathe : MonoBehaviour
{
    [SerializeField] GameObject targetObject;
    [SerializeField]Vector3 breatheIn;
    [SerializeField]Vector3 breatheOut;
    [SerializeField]bool pulsing = false;
    [SerializeField] float expandDuration = 1.0f;
    private bool breathingIn = true;
    private float currentTime = 0f;

    private void Awake()
    {
        if (!targetObject)
        {
            targetObject = gameObject;
        }
    }
    // Update is called once per frame
    void Update()
    {
        PulseUpdate();
    }

    private void PulseUpdate()
    {
        if (pulsing)
        {
            Vector3 targetScale = breathingIn ? breatheIn : breatheOut;
            Vector3 startScale = breathingIn ? breatheOut : breatheIn;
            currentTime += Time.deltaTime;
            float lerpFactor = currentTime / expandDuration;

            targetObject.transform.localScale = Vector3.Lerp(startScale, targetScale, lerpFactor);

            if(lerpFactor >= 1.0f)
            {
                breathingIn = !breathingIn;
                currentTime = 0f;
            }
        }
    }
}
