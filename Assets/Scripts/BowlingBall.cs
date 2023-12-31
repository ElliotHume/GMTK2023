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

    public MeshRenderer renderer;
    public Color damagedTint = Color.gray;
    
    public UnityEvent OnBreak;
    public UnityEvent OnBallCollide;
    public UnityEvent OnObstacleCollide;
    public UnityEvent OnGutter;

    private bool _isBumped;
    private bool _isBroken;
    private bool _isGuttered;
    private bool _isGrounded;
    private Vector3 _gutterDirection;
    private Vector3 _ricochetDirection;
    private float _floorHeight;

    [HideInInspector]
    public float speed;
    [HideInInspector]
    public int damage;
    [HideInInspector]
    public int health;

    void Reset()
    {
        renderer = GetComponent<MeshRenderer>();
    }

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

        if (!_isGrounded || (_floorHeight != 0 && Math.Abs(transform.position.y - _floorHeight) > 0.01f))
        {
            transform.position -= Vector3.up * Time.deltaTime * 3f;
            if (_floorHeight != 0 && transform.position.y < _floorHeight)
                transform.position += Vector3.up * (_floorHeight - transform.position.y);
        }
    }

    private void Ricochet(Vector3 hitNormal, float bounciness = 1f)
    {
        _isBumped = true;
        _ricochetDirection = hitNormal * bounciness;
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
            _floorHeight = transform.position.y;
        }
        else if (tag == "Gutter")
        {
            OnGutter.Invoke();
            
            Gutter(collisionGO.transform);
        } else if (tag == "Obstacle")
        {
            OnObstacleCollide.Invoke();

            Obstacle obstacle = collisionGO.GetComponent<Obstacle>();
            if (damage > 0)
            {
                int damageDealt = obstacle.Hit(damage);
                damage -= damageDealt;

                // bouncy obstacles still bounce even if they dont break the ball
                if (obstacle.bounciness > 5 && damage > 0)
                {
                    Ricochet(collision.contacts[0].normal, obstacle.bounciness);
                } 
            }
            
            if (damage <= 0)
            {
                Ricochet(collision.contacts[0].normal, obstacle.bounciness);
                health -= 1;
                
                if (health <= 0)
                    Break();
            }
        } else if (tag == "NPC")
        {
            OnObstacleCollide.Invoke();
            
            NPC npc = collisionGO.GetComponent<NPC>();
            npc.Hit();
            
            damage -= 1;
            
            Ricochet(collision.contacts[0].normal);
        } else if (tag == "BowlingBall")
        {
            OnBallCollide.Invoke();
            
            BowlingBall ball = collisionGO.GetComponent<BowlingBall>();
            
            damage -= ball.damage;

            Ricochet(collision.contacts[0].normal);
        } else if (tag == "Player")
        {
            OnObstacleCollide.Invoke();
            
            Player player = collisionGO.GetComponent<Player>();
            player.BowlingBallHit();
            
            damage -= 1;
            
            Ricochet(collision.contacts[0].normal);
        } else if (tag == "Despawn")
        {
            Destroy(gameObject);
        }

        if (damage <= 0 && renderer != null)
        {
            renderer.material.color = damagedTint;
        }
    }


    private void DestroySelf()
    {
        Destroy(gameObject);
    }
}
