using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Obstacle : MonoBehaviour
{
    public ObstacleStats stats;

    public GameObject brokenPrefab;
    
    public UnityEvent OnPlace;
    public UnityEvent OnHit;
    public UnityEvent OnBreak;


    [HideInInspector]
    public int health;
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        health = stats.health;
    }

    public void Place()
    {
        OnPlace.Invoke();
    }

    public int Hit(int damage)
    {
        OnHit.Invoke();

        int damageDealt = health < damage ? health : damage;
        
        health -= damageDealt;
        if (health <= 0)
        {
            Break();
        }

        return damageDealt;
    }

    private void Break()
    {
        OnBreak.Invoke();
        if (brokenPrefab != null)
            Instantiate(brokenPrefab, transform.position, transform.rotation);
        Destroy(this);
    }
    
    
}
