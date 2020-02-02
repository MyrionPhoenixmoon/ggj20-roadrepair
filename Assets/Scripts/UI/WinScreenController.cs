using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Threading;

public class WinScreenController : MonoBehaviour
{

    [SerializeField]
    private Text text;
    private Timer sceneTransitionTimer;
    public int sceneTransitionDelay;
    private bool sceneTransitionTrigger = false;

    // Start is called before the first frame update
    void Start()
    {
        this.text.text = RunTimer.finalTime;
        this.sceneTransitionTimer = new Timer(this.SetTransitionTrigger, null, sceneTransitionDelay * 1000, -1);
    }

    // Update is called once per frame
    void Update()
    {
        if (sceneTransitionTrigger) { SceneManager.LoadScene(0); }
    }

    private void SetTransitionTrigger(object state)
    {
        this.sceneTransitionTrigger = true;
    }
}
