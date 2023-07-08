using System;
using UnityEngine;
using UnityEngine.Events;

public class PlayerMovementController : MonoBehaviour
{

    public float floorHeight = 0.25f;
    public Animator animator;
    public GameObject characterRoot;
    public CharacterController characterController;

    private bool _applyGravity = true;
    private bool _isKnockedDown;

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

        characterController.Move( direction * Time.deltaTime);
        characterRoot.transform.rotation = Quaternion.LookRotation(direction);

        if (transform.position.y > floorHeight)
        {
            characterController.Move(Vector3.down * Time.deltaTime);
            if (transform.position.y < floorHeight) characterController.Move( Vector3.up * (floorHeight - transform.position.y));
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.tag == "BowlingBall")
        {
            _applyGravity = false;
            Invoke(nameof(ResetGravity), 1f);
        }
    }

    private void ResetGravity()
    {
        _applyGravity = true;
    }
}
