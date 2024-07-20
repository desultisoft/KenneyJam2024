using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;

public class PostProcessingController : MonoBehaviour
{
    public static PostProcessingController PostProcessingSingleton;
    private PostProcessVolume volume;

    private float chromaticTarget;
    private float chromaticProgress;
    private float chromaticDuration;
    [SerializeField] private float chromaticSpeed;
    private float chromaticDirection = 1;
    private bool chromaticStarted;
    private ChromaticAberration chromaticAberration;

    private float cameraShakeIntensity;
    private float cameraShakeDuration;
    private float cameraShakeProgress;
    private Vector3 cameraStartPosition;
    private Transform cameraTransform;

    private void Awake()
    {
        PostProcessingSingleton = this;
    }
    void Start()
    {
        volume = GetComponent<PostProcessVolume>();
        chromaticAberration = volume.profile.GetSetting<ChromaticAberration>();
        cameraTransform = Camera.main.transform;
        cameraStartPosition = cameraTransform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        UpdateChromaticEffect();
        UpdateCameraShake();
    }

    public void StartChromaticEffect(float intensity, float duration)
    {
        if (intensity < chromaticTarget) return;
        chromaticTarget = intensity;
        chromaticStarted = true;
        chromaticDuration = duration;
        chromaticDirection = 1;
    }

    private void UpdateChromaticEffect()
    {
        if (chromaticStarted == false) return;
        chromaticProgress += chromaticSpeed * chromaticDirection;
        if (chromaticProgress >= chromaticDuration)
        {
            chromaticProgress = chromaticDuration;
            chromaticDirection = -1;
        }
        else if (chromaticProgress < 0)
        {
            chromaticProgress = 0;
            chromaticDirection = 1;
            chromaticStarted = false;
        }
        chromaticAberration.intensity.value = Mathf.Lerp(0.0f,chromaticTarget,chromaticProgress / chromaticDuration);
    }

    public void StartCameraShake(float intensity, float duration)
    {
        if (intensity < cameraShakeIntensity) return;
        cameraShakeIntensity = intensity;
        cameraShakeDuration = duration;
        cameraShakeProgress = 0;
    }

    private void UpdateCameraShake()
    {
        if (cameraShakeDuration == 0) return;
        cameraShakeProgress += Time.fixedDeltaTime;
        Vector3 cameraOffset = Vector3.right * Random.Range(-cameraShakeIntensity, cameraShakeIntensity) + Vector3.up * Random.Range(-cameraShakeIntensity, cameraShakeIntensity);
        if (cameraShakeDuration <= cameraShakeProgress)
        {
            cameraShakeDuration = 0;
            cameraTransform.position = cameraStartPosition;
            cameraShakeIntensity = 0;
            cameraOffset = Vector3.zero;
            cameraShakeProgress = 0;
        }
        cameraTransform.position = cameraStartPosition + cameraOffset;
    }
}
