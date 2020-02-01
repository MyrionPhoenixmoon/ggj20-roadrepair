using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public GameObject Repaired;
    public GameObject Broken;
    public ObstacleType Type;



    public void Repair() {
        if (Repaired!=null) {
            Repaired.SetActive(true);
        }
        if (Broken!=null) {
            Destroy(Broken.gameObject);
        }
    }
}


public enum ObstacleType {
    Powerup,
    Crash,
    Drift,
    Slowdown,
}