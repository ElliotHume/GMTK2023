using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSpawner : MonoBehaviour
{
    public float maxTimeBetweenBalls = 8;
    public float minTimeBetweenBalls = 1;
    public List<Transform> spawnAnchors;

    public List<GameObject> ballPrefabs;


    private float _cooldown;
    // Start is called before the first frame update
    void Start()
    {
        _cooldown = Random.Range(minTimeBetweenBalls, maxTimeBetweenBalls);
    }

    // Update is called once per frame
    void Update()
    {
        if (_cooldown <= 0)
        {
            int randomSpawnAnchorIndex = Random.Range(0, spawnAnchors.Count);
            int randomBallIndex = Random.Range(0, ballPrefabs.Count);

            GameObject ballPrefab = ballPrefabs[randomBallIndex];
            Transform spawnAnchor = spawnAnchors[randomSpawnAnchorIndex];
            
            GameObject newBall = Instantiate(ballPrefab, spawnAnchor.position, spawnAnchor.rotation);

            _cooldown = Random.Range(minTimeBetweenBalls, maxTimeBetweenBalls);
        }
        else
        {
            _cooldown -= Time.deltaTime;
        }
        
    }
}
