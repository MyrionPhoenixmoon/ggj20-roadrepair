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

    public float ObstacleProbability = 0.5f;

    public float DecoProbability = 0.5f;

    // Should be from left to right
    public ObstaclePosition[] PotentialObstacles;

    public DecoPosition[] PotentialDecos;

    List<Obstacle> spawnedObstacles = new List<Obstacle>();

    int currentObstacle = 0;

    //initialize this part of the road (difficulty might later be used)
    public void Init(float difficulty = 0) {
        float rnd = 0;
        int intRND = 0;
        //*
        for (int i = 0; i < PotentialObstacles.Length; i++) {
            rnd = Random.Range(0, 1.0f);
            if (rnd<= ObstacleProbability) {
                intRND = Random.Range(0, PotentialObstacles[i].PotentialObstacle.Length);
                Obstacle obstacle = Instantiate(PotentialObstacles[i].PotentialObstacle[intRND]);
                obstacle.transform.parent = this.transform;
                obstacle.transform.position = PotentialObstacles[i].Position.position;
                spawnedObstacles.Add(obstacle);
            }
        }
        //*/

        for (int i = 0; i < PotentialDecos.Length; i++) {
            rnd = Random.Range(0, 1.0f);
            if (rnd <= DecoProbability) {
                intRND = Random.Range(0, PotentialDecos[i].PotentialDecoElements.Length);
                DecoElement obstacle = Instantiate(PotentialDecos[i].PotentialDecoElements[intRND]);
                obstacle.transform.parent = this.transform;
                obstacle.transform.position = PotentialDecos[i].Position.position;
            }
        }
    }

    //Clear the object (delete it, or better use object pool).
    public void Clear() {

        Destroy(this.gameObject);
    }

    public Obstacle GetNextObstacle() {
        if (spawnedObstacles.Count<=currentObstacle) {
            return null;
        } else {
            return spawnedObstacles[currentObstacle];
        }
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
    public DecoElement[] PotentialDecoElements;

    //Spawn the obstacle (difficultyy might later be used).
    public void Spawn(float difficulty = 0) {

    }

}
