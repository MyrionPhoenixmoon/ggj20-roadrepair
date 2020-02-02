using System.Collections.Generic;

using UnityEngine;

public class RoadPart : MonoBehaviour
{
    public float DecoProbability = 0.5f;

    // How long is the fragment (ideally all have the same length, but we will see)
    public float Length = 20;

    public float ObstacleProbability = 0.5f;

    public DecoPosition[] PotentialDecos;

    // Should be from left to right
    public ObstaclePosition[] PotentialObstacles;

    // which lane does the fragment end
    public LaneNumber StreetEnd;

    // which lane does the fragment start
    public LaneNumber StreetStart;

    int currentObstacle = 0;

    List<Obstacle> spawnedObstacles = new List<Obstacle>();

    public float RoadEnd
    {
        get
        {
            return this.transform.position.x + this.Length / 2;
        }
    }

    public float RoadStart
    {
        get
        {
            return this.transform.position.x - this.Length / 2;
        }
    }

    // Clear the object (delete it, or better use object pool).
    public void Clear()
    {
        Destroy(this.gameObject);
    }

    public Obstacle GetNextObstacle()
    {
        if (this.spawnedObstacles.Count <= this.currentObstacle)
        {
            return null;
        }
        else
        {
            return this.spawnedObstacles[this.currentObstacle];
        }
    }

    // initialize this part of the road (difficulty might later be used)
    public void Init(float difficulty = 0)
    {
        float rnd = 0;
        int intRND = 0;

        // *
        foreach (var t in this.PotentialObstacles)
        {
            rnd = Random.Range(0, 1.0f);
            if (rnd <= this.ObstacleProbability)
            {
                intRND = Random.Range(0, t.PotentialObstacle.Length);
                Obstacle obstacle = Instantiate(t.PotentialObstacle[intRND]);
                obstacle.transform.parent = this.transform;
                obstacle.transform.position = t.Position.position;
                this.spawnedObstacles.Add(obstacle);
            }
        }

        // */
        for (int i = 0; i < this.PotentialDecos.Length; i++)
        {
            rnd = Random.Range(0, 1.0f);
            if (rnd <= this.DecoProbability)
            {
                intRND = Random.Range(0, this.PotentialDecos[i].PotentialDecoElements.Length);
                DecoElement obstacle = Instantiate(this.PotentialDecos[i].PotentialDecoElements[intRND]);
                obstacle.transform.parent = this.transform;
                obstacle.transform.position = this.PotentialDecos[i].Position.position;
            }
        }
    }
}

[System.Serializable]
public class ObstaclePosition
{
    // Where can it spawn?
    public Transform Position;

    // Which obstacles can be spawned?
    public Obstacle[] PotentialObstacle;

    // Spawn the obstacle (difficultyy might later be used).
    public void Spawn(float difficulty = 0)
    {
    }
}

public enum LaneNumber
{
    first,

    second,

    third,

    forth,

    fifth
}

[System.Serializable]
public class DecoPosition
{
    // Where can it spawn?
    public Transform Position;

    // Which deko elements can be spawned?
    public DecoElement[] PotentialDecoElements;

    // Spawn the obstacle (difficultyy might later be used).
    public void Spawn(float difficulty = 0)
    {
    }
}