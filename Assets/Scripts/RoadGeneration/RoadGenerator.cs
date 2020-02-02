using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadGenerator : MonoBehaviour {


    public float FrontRoadLength = 50;
    public float BackRoadLength = 20;

    float PlayerPosition;
    public float Start;
    public float End;

    public float MinObstacleDistance = 50;
    public float MaxObstacleDistance = 150;

    public Transform Marker;

    public Transform PlayerCar;
    public RoadPart[] StartParts;
    public RoadType[] RoadTypes;

    public int NumberOfElements = 50;

    public RoadPart[] EndParts;


    public RoadPart lastRoad;
    public RoadPart fistRoad;

    public RoadPart currentRoad;

    int numberOfElements;

    public Obstacle CurrentObstacle {
        get {
            if (currentObstacle==null && currentObstacle.transform.position.x - PlayerPosition <=MaxObstacleDistance) {
                return currentObstacle;
            } else {
                return null;
            }
        }
    }

    List<RoadPart> roadParts = new List<RoadPart>();

    Obstacle currentObstacle = null;
    List<RoadPart> activeRoadParts = new List<RoadPart>();


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
        checkForNewObstacle();
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
        if (numberOfElements < NumberOfElements) {


            Debug.Log("Spawn road");
            int roadType = (int)lastRoad.StreetEnd;
            int rnd = Random.Range(0, RoadTypes[roadType].PossibleRoads.Length);
            RoadPart newPart = Instantiate(RoadTypes[roadType].PossibleRoads[rnd]);
            roadParts.Add(newPart);
            activeRoadParts.Add(newPart);
            numberOfElements++;
            newPart.transform.parent = this.transform;
            newPart.transform.position = new Vector3(lastRoad.transform.position.x + lastRoad.Length / 2 + newPart.Length / 2, 0, 0);
            lastRoad = roadParts[roadParts.Count - 1];
            newPart.Init();
        } else if (NumberOfElements==numberOfElements) {
            int roadType = (int)lastRoad.StreetEnd;
            RoadPart newPart = Instantiate(EndParts[roadType]);
            roadParts.Add(newPart);
            activeRoadParts.Add(newPart);
            numberOfElements++;
            newPart.transform.parent = this.transform;
            newPart.transform.position = new Vector3(lastRoad.transform.position.x + lastRoad.Length / 2 + newPart.Length / 2, 0, 0);
            lastRoad = roadParts[roadParts.Count - 1];

            numberOfElements++;

        }
    }

    void ClearRoad() {
        Debug.Log("Clear Road");
        roadParts[0].Clear();
        roadParts.Remove(roadParts[0]);
        fistRoad = roadParts[0];
    }

    void checkForNewObstacle() {

        if (currentObstacle==null) {
            GetNextObstacle();
        } else {
            if (currentObstacle.transform.position.x-PlayerPosition <= MinObstacleDistance) {
                GetNextObstacle();
            }
        }
        
    }

    void GetNextObstacle() {
        if (activeRoadParts.Count < 1) {
            return;
        }
        currentObstacle = activeRoadParts[0].GetNextObstacle();
        if (currentObstacle == null) {
            activeRoadParts.Remove(activeRoadParts[0]);
            GetNextObstacle();
        } else {
            Marker.position = currentObstacle.transform.position;
        }
        
    }
}

[System.Serializable]
public class RoadType {
    public LaneNumber StreetStart;
    public RoadPart[] PossibleRoads;
}