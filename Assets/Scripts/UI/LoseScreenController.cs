using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading;

public class LoseScreenController : MonoBehaviour
{
    private Timer transitionTimer;
    private bool timedOut;

    public int TransitionWaitTime = 10;

    // Start is called before the first frame update
    void Start()
    {
        //Play Defeat sounds
        
        timedOut = false;
        this.transitionTimer = new Timer(this.GoToStart);
        this.transitionTimer.Change(this.TransitionWaitTime * 1000, 0);
        
    }

    // Update is called once per frame
    void Update()
    {
        if (timedOut) { SceneManager.LoadScene(0); }
    }

    private void GoToStart(object state){
        timedOut = true;
    }
}
