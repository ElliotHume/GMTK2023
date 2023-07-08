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
    
    public UnityEvent OnPickup;
    public UnityEvent OnPlace;
    public UnityEvent OnHit;
    public UnityEvent OnBreak;
    
    [HideInInspector]
    public int health;

    private bool _isBeingGrabbed;
    private bool _isGrounded;
    private float _floorHeight;

    // Start is called before the first frame update
    void Start()
    {
        health = stats.health;
    }

    void Update()
    {
        if (_isBeingGrabbed && !Input.GetKey("e"))
        {
            Place();
        }
        
        // if (!_isGrounded || (_floorHeight != 0 && transform.position.y > _floorHeight))
        // {
        //     transform.position -= Vector3.up * Time.deltaTime * 3f;
        // }
    }

    public void Pickup()
    {
        OnPickup.Invoke();
        _isBeingGrabbed = true;
        transform.parent = Player.Instance.characterRoot.transform;
    }
    
    public void Place()
    {
        _isBeingGrabbed = false;
        transform.parent = null;
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
                Pickup();
                
            }
        }
    }
}
