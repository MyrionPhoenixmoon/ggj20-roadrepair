using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Driving;

public class MPHReader : MonoBehaviour
{
    private Car car;
    [SerializeField]
    private Text text;

    // Start is called before the first frame update
    void Start()
    {
        car = GameObject.FindObjectOfType<Car>();
        
    }

    // Update is called once per frame
    void Update()
    {
        this.text.text = car.Speed.ToString();
    }
}
