using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadPart : MonoBehaviour {

    //which lane does the fragment start
    public LaneNumber StreetStart;
    //which lane does the fragment end
    public LaneNumber StreetEnd;

    //How long is the fragment (ideally all have the same length, but we will see)
    public float Length = 20;

    public ObstaclePosition[] PotentialObstacles;

    public DecoPosition[] PotentialDekos;


    //initialize this part of the road (difficulty might later be used)
    public void Init(float difficulty = 0) {

    }

    //Clear the object (delete it, or better use object pool).
    public void Clear() {

        Destroy(this.gameObject);
    }

    public float RoadStart {
        get {
            return this.transform.position.x - Length / 2;
        }
    }

    public float RoadEnd {
        get {
            return this.transform.position.x + Length / 2;
        }
    }
}

[System.Serializable]
public class ObstaclePosition {
    // Where can it spawn?
    public Transform Position;
    // Which obstacles can be spawned?
    public Obstacle[] PotentialObstacle;

    //Spawn the obstacle (difficultyy might later be used).
    public void Spawn(float difficulty = 0) {

    }

}


public enum LaneNumber {
    first,
    second,
    third,
    forth,
    fifth
}

[System.Serializable]
public class DecoPosition {
    // Where can it spawn?
    public Transform Position;
    // Which deko elements can be spawned?
    public DecoElement[] PotentialDekoElements;

    //Spawn the obstacle (difficultyy might later be used).
    public void Spawn(float difficulty = 0) {

    }

}
