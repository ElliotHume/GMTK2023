using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public float maxTimeBetweenObstacles = 8;
    public float minTimeBetweenObstacles = 1;
    public List<Transform> spawnAnchors;

    public List<GameObject> obstaclePrefabs;


    private float _cooldown;
    // Start is called before the first frame update
    void Start()
    {
        _cooldown = Random.Range(minTimeBetweenObstacles, maxTimeBetweenObstacles);
    }

    // Update is called once per frame
    void Update()
    {
        if (_cooldown <= 0)
        {
            int randomSpawnAnchorIndex = Random.Range(0, spawnAnchors.Count);
            int randomBallIndex = Random.Range(0, obstaclePrefabs.Count);

            GameObject ballPrefab = obstaclePrefabs[randomBallIndex];
            Transform spawnAnchor = spawnAnchors[randomSpawnAnchorIndex];
            
            GameObject newObstacle = Instantiate(ballPrefab, spawnAnchor.position, spawnAnchor.rotation);

            _cooldown = Random.Range(minTimeBetweenObstacles, maxTimeBetweenObstacles);
        }
        else
        {
            _cooldown -= Time.deltaTime;
        }
        
    }
}
