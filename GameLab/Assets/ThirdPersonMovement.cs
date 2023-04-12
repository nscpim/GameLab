using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonMovement : MonoBehaviour
{
    public CharacterController controller;
    public float speed = 6f;
    public Transform cam; 
    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;
    public float jumpSpeed = 2.0f;
    public float gravity = 10.0f;
    private Vector3 movingDirection = Vector3.zero;
    public float maxSpeed = 70f;
    public float acceleration = 5;
    public LayerMask wallLayerMask;
    
    private bool isCollidingWithWall = false;




    void Start()
    {
        controller = GetComponent<CharacterController>();
    }
    
    
    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3 (horizontal, 0f, vertical).normalized;

        if (Input.GetButton("Jump"))
        {
            if (controller.isGrounded)
            {
                movingDirection.y = jumpSpeed;
            }     
        }    
        movingDirection.y -= gravity * Time.deltaTime;
        controller.Move(movingDirection * Time.deltaTime);

        if (speed < maxSpeed)
        {
            speed += acceleration * Time.deltaTime;
        }

        if (!this.transform.hasChanged)
        {
            speed = 100f;
        }
        transform.hasChanged = false;

        //transform.position.x = transform.position.x + speed*Time.deltaTime;

        

        if (direction.magnitude >= 0.1f)
        {   
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * speed * Time.deltaTime);
        }



    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        
        if (wallLayerMask == (wallLayerMask | (1 << hit.gameObject.layer)))
        {
            
            Debug.Log("Collided with a wall!");
            isCollidingWithWall = true;
            gravity = 0f;


        }
    }

    void FixedUpdate()
    {
        
        if (isCollidingWithWall && !controller.isGrounded)
        {
            
            Debug.Log("Stopped colliding with a wall!");
            gravity = 200f;
            isCollidingWithWall = false;
        }
    }





}
