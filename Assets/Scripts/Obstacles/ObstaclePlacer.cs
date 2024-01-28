using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct ObstacleWithPriority
{
    public Obstacle obstacle;
    public int priority;
}

public class ObstaclePlacer : MonoBehaviour
{
    [SerializeField] private GameObject target;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask obstacleLayer;
    [SerializeField] private ObstacleWithPriority[] obstacles;
    [SerializeField] private Vector3 positionOffset;
    [SerializeField] private float distanceThreshold = 10f;
    [SerializeField] private float xLine = 5f;
    
    private Vector3 _lastPosition;
    private List<GameObject> _obstacles = new List<GameObject>();

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + positionOffset);
    }

    private void Start()
    {
        StartCoroutine(SpawnObstacles());
    }
    
    private IEnumerator SpawnObstacles()
    {
        while (true)
        {
            if (Vector3.Distance(target.transform.position, _lastPosition) >= UnityEngine.Random.Range(distanceThreshold, distanceThreshold + 5f))
            {
                RandomObstacle();
                _lastPosition = target.transform.position;
            }
            yield return null;
        }
    }

    public void RandomObstacle()
    {
        var randomObstacle = GetRandomObstacle();
        var randomRotation = UnityEngine.Random.Range(0, 360);
        var randomX = UnityEngine.Random.Range(-xLine, xLine);
        var position = new Vector3(randomX, target.transform.position.y + positionOffset.y, target.transform.position.z + positionOffset.z);
        if (Physics.Raycast(position, Vector3.down, out var hit, 100f, groundLayer))
        {
            if (Physics.OverlapBox(hit.point, randomObstacle.transform.localScale / 2, Quaternion.identity, obstacleLayer).Length == 0)
            {
                var rotation = randomObstacle.dontChangeRotation ? Quaternion.Euler(0, randomRotation, 0) : Quaternion.FromToRotation(Vector3.up, hit.normal) * Quaternion.Euler(0, randomRotation, 0);
                var clone = Instantiate(randomObstacle.gameObject, hit.point + randomObstacle.gameObject.transform.position, rotation);
                _obstacles.Add(clone);
                StartCoroutine(clone.GetComponent<Obstacle>().OnPlaced(target));
            }
        }
    }

    private Obstacle GetRandomObstacle()
    {
        var totalPriority = 0;
        foreach (var obstacle in obstacles)
        {
            totalPriority += obstacle.priority;
        }
        
        var random = UnityEngine.Random.Range(0, totalPriority);
        var current = 0;
        foreach (var obstacle in obstacles)
        {
            current += obstacle.priority;
            if (random < current)
            {
                return obstacle.obstacle;
            }
        }
        return null;
    }
}