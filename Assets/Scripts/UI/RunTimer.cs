using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Diagnostics;

public class RunTimer : MonoBehaviour
{

    [SerializeField]
    private Text text;

    private Stopwatch timer = new Stopwatch();

    public static string finalTime;
    // Start is called before the first frame update
    void Start()
    {
        timer.Start();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.frameCount % 2 == 0){
            // Get the elapsed time as a TimeSpan value.
            TimeSpan ts = timer.Elapsed;
            string elapsedTime = String.Format("{0:00} : {1:00} . {2:00}",
                ts.Minutes, ts.Seconds,
                ts.Milliseconds / 10);

            this.text.text = elapsedTime;
            RunTimer.finalTime = elapsedTime;
        }
    }
}
