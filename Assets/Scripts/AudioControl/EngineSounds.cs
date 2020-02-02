using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class EngineSounds : MonoBehaviour
{
    public AudioClip engineIdle;
    public AudioMixer audioMixer;
    public AudioSource audioSource; 
    // Start is called before the first frame update
    void Start()
    {
        playEngineSound();
    }

    // Update is called once per frame
    void Update()
    {
        //audioMixer.
        audioSource.pitch = Input.GetAxis("Vertical") + 1.3f;
        audioSource.volume = Input.GetAxis("Vertical") + 0.5f;
    }

    void playEngineSound()
    {
        //audioSource.PlayOneShot(engine1);
        audioSource.clip = engineIdle;
        audioSource.loop = true;
        audioSource.Play();
    }
}
