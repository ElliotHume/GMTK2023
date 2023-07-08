using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class NPC : MonoBehaviour
{
    public CapsuleCollider collider;

    public Animator animator;

    public Player player;

    public float pickupRadius = 0.5f;
    public float timeToPickup = 2f;
    public Slider pickupSlider;

    public GameObject deathPrefab;

    public UnityEvent OnPickedUp;
    public UnityEvent OnKnockedOver;
    public UnityEvent OnDeath;
    
    [HideInInspector]
    public bool isKnockedOver;

    [HideInInspector]
    public bool isDead;

    private float _pickupProgress;
    private Vector3 _initialPosition;
    private Quaternion _intialRotation;

    void Start()
    {
        _initialPosition = transform.position;
        _intialRotation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetBool("KnockedOver", isKnockedOver);
        animator.SetBool("Dead", isDead);

        if (isDead)
            return;
        
        if (isKnockedOver && Input.GetKey("q") && Vector3.Distance(player.transform.position, transform.position) < pickupRadius && player.canMove)
        {
            _pickupProgress += Time.deltaTime;
        }
        
        if (pickupSlider != null)
        {
            pickupSlider.gameObject.SetActive(!isDead && isKnockedOver && _pickupProgress != 0);
            pickupSlider.value = _pickupProgress / timeToPickup;
        }

        if (_pickupProgress >= timeToPickup)
        {
            PickUp();
        }

        transform.position = _initialPosition;
        transform.rotation = _intialRotation;
    }

    public void Hit()
    {
        _pickupProgress = 0f;
        if (!isKnockedOver)
        {
            isKnockedOver = true;
            OnKnockedOver.Invoke();
        }
        else
        {
            isDead = true;
            collider.enabled = false;
            OnDeath.Invoke();
            if (deathPrefab != null)
                Instantiate(deathPrefab, transform.position, transform.rotation);
            if (pickupSlider != null)
                pickupSlider.gameObject.SetActive(false);
        }
    }

    public void PickUp()
    {
        if (isDead || !isKnockedOver) return;
        OnPickedUp.Invoke();
        isKnockedOver = false;
        _pickupProgress = 0f;
    }

    private void OnDrawGizmos()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, pickupRadius);
    }
}
