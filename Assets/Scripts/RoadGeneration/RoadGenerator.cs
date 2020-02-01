using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadGenerator : MonoBehaviour {


    public float FrontRoadLength = 50;
    public float BackRoadLength = 20;

    public Transform PlayerCar;
    public RoadPart[] StartParts;
    public RoadType[] RoadTypes;


    RoadPart lastRoad;
    RoadPart fistRoad;

    List<RoadPart> roadParts;

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

        //Spawn new road part when needed
        if (lastRoad.RoadEnd + FrontRoadLength < playerPosition) {
            SpawnNewRoad();
        }

        //Despawn road part when no longer needed
        if (fistRoad.RoadEnd + BackRoadLength < playerPosition) {
            ClearRoad();
        }
    }

    void SpawnNewRoad() {

    }

    void ClearRoad() {

    }

    List<RoadPart> SpawnedRoad = new List<RoadPart>();


}

[System.Serializable]
public class RoadType {
    public LaneNumber StreetStart;
    public RoadPart[] PossibleRoads;
}