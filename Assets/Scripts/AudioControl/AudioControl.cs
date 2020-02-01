using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioControl : MonoBehaviour
{
    public AudioClip engine1;
    public AudioClip engine2;
    public AudioClip crash;
    public AudioClip engineIdle;

    public AudioSource audioSource; 
    // Start is called before the first frame update
    void Start()
    {
        playEngineSound();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void playEngineSound()
    {
        audioSource.PlayOneShot(engine1);
    }
}
