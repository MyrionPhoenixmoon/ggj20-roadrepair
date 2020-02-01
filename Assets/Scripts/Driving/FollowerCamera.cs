using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowerCamera : MonoBehaviour {

    public Transform Target;

    Vector3 StartPosition;

    void Start() {
        StartPosition = this.transform.position;
    }

    // Update is called once per frame
    void Update() {
        this.transform.position = new Vector3(Target.position.x, StartPosition.y, StartPosition.z);

    }
}
