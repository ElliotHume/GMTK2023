using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSpawner : MonoBehaviour
{
    public float maxTimeBetweenBalls = 8;
    public float minTimeBetweenBalls = 1;
    public List<Transform> spawnAnchors;

    public List<GameObject> ballPrefabs;

    public float timeReductionPerMinute;

    private float _cooldown;

    private float _minTime;
    private float _maxTime;
    private float _scalingTimeReduction;
    
    // Start is called before the first frame update
    void Start()
    {
        _cooldown = Random.Range(minTimeBetweenBalls, maxTimeBetweenBalls);
        _minTime = minTimeBetweenBalls;
        _maxTime = maxTimeBetweenBalls;
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
            
            Instantiate(ballPrefab, spawnAnchor.position, spawnAnchor.rotation);

            _cooldown = Random.Range(_minTime, _maxTime);
        }
        else
        {
            _cooldown -= Time.deltaTime;
        }

        if (timeReductionPerMinute > 0f)
        {
            _minTime -= timeReductionPerMinute * 0.016f * Time.deltaTime;
            _maxTime -= timeReductionPerMinute * 0.016f * Time.deltaTime;

            _minTime = Mathf.Max(1f, _minTime);
            _maxTime = Mathf.Max(1f, _maxTime);
        }
    }
}
