﻿using System.Collections;
using System.Collections.Generic;

using Assets.Scripts.Driving;

using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public GameObject Repaired;
    public GameObject Broken;
    public ObstacleType Type;



    public void Repair()
    {
        if (Repaired != null)
        {
            Repaired.SetActive(true);
        }
        if (Broken != null)
        {
            Destroy(Broken.gameObject);
        }
    }

    public void OnTriggerEnter(Collider collider)
    {
        var collidingObject = collider.GetComponent<Car>();
        collidingObject?.Triggered(this);
    }
}


public enum ObstacleType
{
    Powerup,
    Crash,
    Drift,
    Slowdown,
}