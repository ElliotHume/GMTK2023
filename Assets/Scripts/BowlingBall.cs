using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(SphereCollider), typeof(Rigidbody))]
public class BowlingBall : MonoBehaviour
{
    public BowlingBallStats stats;

    public GameObject spinningModel;
    public float spinningMultiplier = 3f;
    public float bounceMultiplier = 1f;

    public GameObject breakEffectPrefab;
    
    public UnityEvent OnBreak;
    public UnityEvent OnCollide;
    public UnityEvent OnGutter;

    private bool _isBumped;
    private bool _isBroken;
    private bool _isGuttered;
    private bool _isGrounded;
    private Vector3 _gutterDirection;
    private Vector3 _ricochetDirection;

    [HideInInspector]
    public float speed;
    [HideInInspector]
    public int damage;
    [HideInInspector]
    public int health;

    // Start is called before the first frame update
    void Start()
    {
        speed = stats.speed;
        damage = stats.damage;
        health = stats.health;
    }

    // Update is called once per frame
    void Update()
    {
        if (_isBroken)
            return;
        
        if (health <= 0 && !_isBroken)
        {
            Break();
            return;
        }
        
        Vector3 movementDirection;
        
        if (_isGuttered && _gutterDirection != Vector3.zero)
        {
            movementDirection = _gutterDirection * (stats.speed * Time.deltaTime);
        }
        else if (!_isBumped)
        {
            movementDirection = transform.forward * (stats.speed * Time.deltaTime);
        }
        else
        {
            movementDirection = _ricochetDirection * (stats.speed * Time.deltaTime);

            _ricochetDirection = Vector3.Lerp(_ricochetDirection, transform.forward, bounceMultiplier * Time.deltaTime);
        }

        transform.position += movementDirection;
        
        spinningModel.transform.LookAt(movementDirection);
        spinningModel.transform.RotateAround(spinningModel.transform.position, spinningModel.transform.right, spinningMultiplier * stats.speed * Time.deltaTime);

        if (!_isGrounded)
        {
            transform.position -= Vector3.up * Time.deltaTime;
        }
    }

    private void Ricochet(Vector3 hitNormal)
    {
        _isBumped = true;
        _ricochetDirection = hitNormal;
    }

    private void Gutter(Transform gutter)
    {
        _isGuttered = true;
        _gutterDirection = gutter.forward;
    }

    private void Break()
    {
        _isBroken = true;
        OnBreak.Invoke();
        if (breakEffectPrefab != null)
            Instantiate(breakEffectPrefab, transform.position, transform.rotation);
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_isGuttered || _isBroken) return;
        
        GameObject collisionGO = collision.gameObject;
        String tag = collisionGO.tag;

        if (tag == "Floor")
        {
            _isGrounded = true;
        }
        else if (tag == "Gutter")
        {
            OnGutter.Invoke();
            
            Gutter(collisionGO.transform);
        } else if (tag == "Obstacle")
        {
            OnCollide.Invoke();
            
            // TODO: Deal damage to Obstacle, if no more damage is remaining, bounce away
        } else if (tag == "NPC")
        {
            OnCollide.Invoke();
            
            NPC npc = collisionGO.GetComponent<NPC>();
            npc.Hit();
            
            damage -= 1;

            if (damage <= 0)
            {
                Ricochet(collision.contacts[0].normal);
            }

        } else if (tag == "BowlingBall")
        {
            OnCollide.Invoke();
            
            BowlingBall ball = collisionGO.GetComponent<BowlingBall>();
            
            damage -= ball.damage;
            
            if (damage <= 0)
            {
                Ricochet(collision.contacts[0].normal);
            }
        } else if (tag == "Player")
        {
            OnCollide.Invoke();
            
            Player player = collisionGO.GetComponent<Player>();
            player.BowlingBallHit();
            
            damage -= 1;

            if (damage <= 0)
            {
                Ricochet(collision.contacts[0].normal);
            }
        } else if (tag == "Despawn")
        {
            Destroy(gameObject);
        }
    }


    private void DestroySelf()
    {
        Destroy(gameObject);
    }
}
