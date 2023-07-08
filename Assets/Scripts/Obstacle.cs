using System;
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

    private bool _isBeingGrabbed;

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
        Destroy(gameObject);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject == Player.Instance.gameObject)
        {
            if (Input.GetKey("e") && !_isBeingGrabbed)
            {
                _isBeingGrabbed = true;
                transform.parent = Player.Instance.characterRoot.transform;
            } else if (_isBeingGrabbed && !Input.GetKey("e"))
            {
                _isBeingGrabbed = false;
                transform.parent = null;
            }
        }
    }
}
