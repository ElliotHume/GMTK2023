using System;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{

    public float floorHeight = 0.25f;
    public Animator animator;
    public GameObject characterRoot;
    public CharacterController characterController;
    
    private bool _isKnockedDown;
    private bool _isGettingUp;
    
    public bool canMove => !_isKnockedDown && !_isGettingUp; 

    public UnityEvent OnHitByBowlingBall;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = Vector3.zero;
        bool moving = Input.GetKey("w") || Input.GetKey("a") || Input.GetKey("s") || Input.GetKey("d");
        if (Input.GetKey("w"))
        {
            direction += transform.forward;
        }
        if (Input.GetKey("s"))
        {
            direction -= transform.forward;
        }
        if (Input.GetKey("d"))
        {
            direction += transform.right;
        }
        if (Input.GetKey("a"))
        {
            direction -= transform.right;
        }
        
        animator.SetBool("Running", moving && !_isKnockedDown);
        animator.SetBool("KnockedDown", _isKnockedDown);

        if (canMove)
        {
            characterController.Move( direction * Time.deltaTime);
            if (direction != Vector3.zero)
                characterRoot.transform.rotation = Quaternion.LookRotation(direction);
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
