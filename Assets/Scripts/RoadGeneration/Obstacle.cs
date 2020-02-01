using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public GameObject Repaired;
    public GameObject Broken;
    public ObstacleType Type;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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