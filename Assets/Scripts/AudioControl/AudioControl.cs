using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioControl : MonoBehaviour
{
    public AudioClip engine1;
    public AudioClip engine2;
    public AudioClip crash;

    public AudioClip skid;
    public AudioClip slowDownGet;
    public AudioClip shieldGet;
    public AudioClip lethalCrash;
    public AudioClip engineIdle;
    public AudioClip engineAccelerate;
    public AudioClip brake;
    public AudioClip gameOver;

    public AudioClip selectTool;

    public AudioClip fixObstacle;
    public AudioClip fixSlowDown;
    public AudioClip fixLethalObstacle;
    public AudioClip fixPowerUp;
    public AudioClip fixOilSlick;
    public AudioClip victory;
    public AudioClip shieldLoop;



    public AudioClip skimWall;

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
