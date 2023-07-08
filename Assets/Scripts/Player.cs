using System;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{

    public float turnSpeed = 10f;
    
    public float floorHeight = 0.25f;
    public Animator animator;
    public GameObject characterRoot;
    public CharacterController characterController;
    
    private bool _isKnockedDown;
    private bool _isGettingUp;
    
    public bool canMove => !_isKnockedDown && !_isGettingUp; 

    public UnityEvent OnHitByBowlingBall;

    private Vector3 _movementDirection;
    
    // Start is called before the first frame update
    void Start()
    {
        _movementDirection = transform.forward;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 inputDirection = Vector3.zero;
        bool moving = Input.GetKey("w") || Input.GetKey("a") || Input.GetKey("s") || Input.GetKey("d");
        if (Input.GetKey("w"))
        {
            inputDirection += transform.forward;
        }
        if (Input.GetKey("s"))
        {
            inputDirection -= transform.forward;
        }
        if (Input.GetKey("d"))
        {
            inputDirection += transform.right;
        }
        if (Input.GetKey("a"))
        {
            inputDirection -= transform.right;
        }
        
        animator.SetBool("Running", moving && !_isKnockedDown);
        animator.SetBool("KnockedDown", _isKnockedDown);

        if (canMove && moving)
        {
            _movementDirection = Vector3.Lerp(_movementDirection, inputDirection, turnSpeed * Time.deltaTime).normalized;
            characterController.Move( _movementDirection * Time.deltaTime);
            if (_movementDirection != Vector3.zero)
                characterRoot.transform.rotation = Quaternion.LookRotation(_movementDirection);
        }
        else
        {
            characterRoot.transform.position =
                Vector3.Lerp(characterRoot.transform.position, transform.position, Time.deltaTime);
        }



        if (Math.Abs(transform.position.y - floorHeight) > 0.01f)
        {
            characterController.enabled = false;
            characterController.transform.position = new Vector3(transform.position.x, floorHeight, transform.position.z);
            characterController.enabled = true;
        }
    }

    public void BowlingBallHit()
    {
        OnHitByBowlingBall.Invoke();
        _isKnockedDown = true;
        _isGettingUp = true;
        Invoke(nameof(ResetAfterCollision), 1f);
        Invoke(nameof(GetUp), 3f);
    }

    public void GetUp()
    {
        _isGettingUp = false;
    }

    private void ResetAfterCollision()
    {
        _isKnockedDown = false;
    }
}
