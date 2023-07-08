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

    public Outline outline;
    
    public UnityEvent OnPickup;
    public UnityEvent OnPlace;
    public UnityEvent OnHit;
    public UnityEvent OnBreak;
    
    [HideInInspector]
    public int health;

    private bool _isBeingGrabbed;
    private bool _isGrounded;
    private float _floorHeight;
    private bool _playerInRange;
    private Player _player;

    // Start is called before the first frame update
    void Start()
    {
        health = stats.health;
        outline.enabled = false;
    }

    void Update()
    {
        
        if (_playerInRange && !_isBeingGrabbed && Input.GetKeyDown("e"))
        {
            Pickup();
        } else if (_isBeingGrabbed && (!_playerInRange || Input.GetKeyDown("e")))
        {
            Place();
        }

        if (_isBeingGrabbed)
        {
            transform.position = _player.pickupPoint.position;
            transform.rotation = _player.pickupPoint.rotation;
        }
    }

    public void Pickup()
    {
        if (_player == null) _player = Player.Instance;
        outline.enabled = true;
        _isBeingGrabbed = true;
        transform.position = _player.pickupPoint.position;
        transform.rotation = _player.pickupPoint.rotation;
        transform.parent = _player.pickupPoint;
        
        OnPickup.Invoke();
    }
    
    public void Place()
    {
        if (_player == null) _player = Player.Instance;
        outline.enabled = false;
        _isBeingGrabbed = false;
        transform.position = _player.placePoint.position;
        transform.rotation = _player.placePoint.rotation;
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == Player.Instance.gameObject)
        {
            _playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == Player.Instance.gameObject)
        {
            _playerInRange = false;
        }
    }
}
