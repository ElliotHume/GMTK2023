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
    
    public UnityEvent OnBreak;
    public UnityEvent OnCollide;
    public UnityEvent OnGutter;

    private bool _isBumped = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 movementDirection = Vector3.zero;
        if (!_isBumped)
        {
            movementDirection = transform.forward * (stats.speed * Time.deltaTime);
            
            spinningModel.transform.RotateAround(spinningModel.transform.position, transform.right, spinningMultiplier * stats.speed * Time.deltaTime);
        }
        else
        {
            
        }

        transform.position += movementDirection;
    }

    private void OnCollisionEnter(Collision other)
    {
        GameObject collisionGO = other.gameObject;
        String tag = collisionGO.tag;

        if (tag == "Gutter")
        {
            OnGutter.Invoke();
            OnCollide.Invoke();
            
            // TODO: Take ball out of play, only move along the gutter until despawn barrier
        } else if (tag == "Obstacle")
        {
            OnCollide.Invoke();
            
            // TODO: Deal damage to Obstacle, if no more damage is remaining, bounce away
        } else if (tag == "NPC")
        {
            OnCollide.Invoke();
            
            // TODO: Knock over the NPC, reduce damage by 1
        } else if (tag == "BowlingBall")
        {
            OnCollide.Invoke();
            
            //TODO: Deal damage to the bowling ball, bounce away if no more damage is remaining OR the colliding bowling ball doesnt die
        } else if (tag == "Player")
        {
            OnCollide.Invoke();
            
            //TODO: Knock down the player, reduce damage by 1
        }
    }
}
