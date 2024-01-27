using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstaclePlacer : MonoBehaviour
{
    [SerializeField] private GameObject target;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask obstacleLayer;
    [SerializeField] private Obstacle[] obstacles;
    [SerializeField] private Vector3 positionOffset;
    [SerializeField] private float xLine = 5f;
    
    private List<GameObject> _obstacles = new List<GameObject>();

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        //draw a line 
        Gizmos.DrawLine(transform.position, transform.position + positionOffset);
    }

    private void Start()
    {
        StartCoroutine(SpawnObstacles());
    }
    
    //spawn obstacles every 2 seconds
    private IEnumerator SpawnObstacles()
    {
        while (true)
        {
            yield return new WaitForSeconds(2f);
            RandomObstacle();
        }
    }

    //spawn random obstacles in a line in front of the player with positionOffset
    public void RandomObstacle()
    {
        var randomObstacle = obstacles[UnityEngine.Random.Range(0, obstacles.Length)];
        var randomRotation = UnityEngine.Random.Range(0, 360);
        var randomX = UnityEngine.Random.Range(-xLine, xLine);
        var position = new Vector3(randomX, target.transform.position.y + positionOffset.y, target.transform.position.z + positionOffset.z);
        //send ray down to find ground
        if (Physics.Raycast(position, Vector3.down, out var hit, 100f, groundLayer))
        {
            //check if is colliding with another obstacle
            if (Physics.OverlapBox(hit.point, randomObstacle.transform.localScale / 2, Quaternion.identity, obstacleLayer).Length == 0)
            {
                var rotation = Quaternion.FromToRotation(Vector3.up, hit.normal) * Quaternion.Euler(0, randomRotation, 0);
                var clone = Instantiate(randomObstacle.gameObject, hit.point, rotation);
                _obstacles.Add(clone);
            }
        }
    }
}
