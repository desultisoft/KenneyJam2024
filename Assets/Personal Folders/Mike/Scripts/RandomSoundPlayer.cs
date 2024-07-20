using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class RandomSoundPlayer : MonoBehaviour
{
    public List<AudioClip> audioClips;
    public float minPitch = 0.8f;
    public float maxPitch = 1.2f;
    public float minVolume = 0.8f;
    public float maxVolume = 1.2f;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayRandomSound()
    {
        if (audioClips.Count == 0)
        {
            Debug.LogWarning("No audio clips assigned!");
            return;
        }

        AudioClip clip = audioClips[Random.Range(0, audioClips.Count)];
        audioSource.pitch = Random.Range(minPitch, maxPitch);
        audioSource.volume = Random.Range(minVolume, maxVolume);
        audioSource.PlayOneShot(clip);
    }
}
