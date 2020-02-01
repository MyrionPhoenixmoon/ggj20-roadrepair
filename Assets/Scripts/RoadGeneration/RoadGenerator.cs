using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadGenerator : MonoBehaviour {


    public float FrontRoadLength = 50;
    public float BackRoadLength = 20;

    public float PlayerPosition;
    public float Start;
    public float End;

    public Transform PlayerCar;
    public RoadPart[] StartParts;
    public RoadType[] RoadTypes;


    public RoadPart lastRoad;
    public RoadPart fistRoad;

    public RoadPart currentRoad;

    List<RoadPart> roadParts = new List<RoadPart>();

    Obstacle currentObstacle = null;

    void Awake() {
        for (int i = 0; i < StartParts.Length; i++) {
            if (StartParts[i]!=null) {
                roadParts.Add(StartParts[i]);
            }
        }
        fistRoad = roadParts[0];
        lastRoad = roadParts[roadParts.Count - 1];
    }


    void Update() {
        CheckRoadSpawn();
    }


    void CheckRoadSpawn() {

        //Despawn roadpart when no longer needed

        float playerPosition = PlayerCar.position.x;
        PlayerPosition = playerPosition;
        Start = fistRoad.RoadEnd;
        End = lastRoad.RoadEnd;

        //Spawn new road part when needed
        if (lastRoad.RoadEnd - FrontRoadLength < playerPosition) {
            SpawnNewRoad();
        }

        //Despawn road part when no longer needed
        if (fistRoad.RoadEnd + BackRoadLength < playerPosition) {
            ClearRoad();
        }
    }

    void SpawnNewRoad() {
        Debug.Log("Spawn road");
        int roadType = (int)lastRoad.StreetEnd;
        int rnd = Random.Range(0, RoadTypes[roadType].PossibleRoads.Length);
        RoadPart newPart = Instantiate(RoadTypes[roadType].PossibleRoads[rnd]);
        roadParts.Add(newPart);
        newPart.transform.parent = this.transform;
        newPart.transform.position = new Vector3(lastRoad.transform.position.x + lastRoad.Length / 2 + newPart.Length/2, 0, 0);
        lastRoad = roadParts[roadParts.Count - 1];
        newPart.Init();
    }

    void ClearRoad() {
        Debug.Log("Clear Road");
        roadParts[0].Clear();
        roadParts.Remove(roadParts[0]);
        fistRoad = roadParts[0];
    }

    List<RoadPart> SpawnedRoad = new List<RoadPart>();

    void GetNextObstacle() {

    }

}

[System.Serializable]
public class RoadType {
    public LaneNumber StreetStart;
    public RoadPart[] PossibleRoads;
}