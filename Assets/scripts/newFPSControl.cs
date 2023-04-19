using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class newFPSControl : MonoBehaviour
{
    float playerHeight = 2f;

    [SerializeField] Transform orientation;

    [Header("shooting")]

    public float health;
    public float healthMax = 5;
    public float healAmmount = 3;

    public float abilityCooldown;
    public float abilityCooldownMax;

    public GameObject grenade;
    public Transform droppoint;
    

    public float dashTime;
    public int doubleJumpCharge;

    [Header("Movement")]
    [SerializeField] float moveSpeed = 6f;
    [SerializeField] float airMultiplier = 0.4f;
    float movementMultiplier = 10f;

    [Header("Sprinting")]
    [SerializeField] float walkSpeed = 10f;
    [SerializeField] float sprintSpeed = 10f;
    [SerializeField] float acceleration = 10f;

    [Header("Jumping")]
    public float jumpForce = 15f;

    [Header("Keybinds")]
    [SerializeField] KeyCode jumpKey = KeyCode.Space;
    [SerializeField] KeyCode sprintKey = KeyCode.LeftShift;

    [Header("Drag")]
    [SerializeField] float groundDrag = 6f;
    [SerializeField] float airDrag = 2f;

    float horizontalMovement;
    float verticalMovement;

    [Header("Ground Detection")]
    //[SerializeField] Transform groundCheck;
    //[SerializeField] LayerMask groundMask;
    //[SerializeField] float groundDistance = 1.1f;
    public bool isGrounded;// { get; private set; }

    Vector3 moveDirection;
    Vector3 slopeMoveDirection;

    Rigidbody rb;

    RaycastHit slopeHit;

    //public Image dashcooldown;


    public enum character
    {
        Aeva,
        //add more here
    }

    public character abl;
    

    private bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight / 2 + 0.5f))
        {
            if (slopeHit.normal != Vector3.up)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        return false;
    }

    public float distToGround;
    private void Start()
    {
        abl = character.Aeva;//change to selector or level specific
        CharCooldowns();

        rb = GetComponent<Rigidbody>();
        Collider cl;
        cl = GetComponent<Collider>();
        distToGround = cl.bounds.extents.y;
        rb.freezeRotation = true;
        doubleJumpCharge = 1;
        dashTime = 5;
        health = healthMax;
    }

    void CharCooldowns()//also sets playermodel eventually
    {
        switch (abl)
        {
            case character.Aeva:
                
                abilityCooldownMax = 10.0f;
                abilityCooldown = abilityCooldownMax;
                break;
            default:
                break;
        }
    }

    
    private void Update()
    {
        //isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        isGrounded = Physics.Raycast(transform.position, -Vector3.up, distToGround + 0.1f);

        MyInput();
        ControlDrag();
        ControlSpeed();

        if (dashTime <= 5)
        {
            dashTime = dashTime + (Time.deltaTime);
        }
        if (Input.GetKeyDown(sprintKey) && dashTime >= 1)
        {
            Dash();
        }

        if (isGrounded == true && doubleJumpCharge <=0)
        {
            doubleJumpCharge = 1;//possibly just add a max
        }

        if (Input.GetKeyDown(jumpKey) && (isGrounded || doubleJumpCharge >= 1))
        {
            Jump();
        }

        slopeMoveDirection = Vector3.ProjectOnPlane(moveDirection, slopeHit.normal);


        GameMan.instance.dashTimeg = dashTime;
        GameMan.instance.playerHealth = health;
        GameMan.instance.playerHealthMax = healthMax;

        if (Input.GetKeyDown(KeyCode.Q) && abilityCooldown >= abilityCooldownMax)
        {
            Ability();
        }
        if (abilityCooldown <= abilityCooldownMax)
        {
            abilityCooldown = abilityCooldown + Time.deltaTime;
        }

    }

    void Ability()
    {
        GameObject G;
        Rigidbody rb;
        switch (abl)
        {
            case character.Aeva:
                G = Instantiate(grenade, droppoint.position, droppoint.rotation);
                rb = G.GetComponent<Rigidbody>();
                rb.AddForce(G.transform.right * -25, ForceMode.Impulse);
                abilityCooldown = 0;
                break;
            default:
                break;
        }
    }



    void MyInput()
    {
        horizontalMovement = Input.GetAxisRaw("Horizontal");
        verticalMovement = Input.GetAxisRaw("Vertical");

        moveDirection = orientation.forward * verticalMovement + orientation.right * horizontalMovement;
    }

    

    private void Dash()
    {
        if (moveDirection.x != 0 && moveDirection.z != 0)
        {
            rb.velocity = new Vector3(0, 0, 0);
            if (isGrounded == true) {
                rb.AddForce(moveDirection * sprintSpeed, ForceMode.Impulse);
            }
            else if(isGrounded == false)
            {
                rb.AddForce(moveDirection * (sprintSpeed / 1.8f), ForceMode.Impulse);
            }
            //dashTime = dashTime - (Time.deltaTime * 7);
            dashTime = dashTime - 2;
        }

    }

    void Jump()
    {
        if (isGrounded)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
            
            
        }
        if(!isGrounded & doubleJumpCharge >= 1 & dashTime >= 2)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
            if (doubleJumpCharge >= 1)
            {
                doubleJumpCharge--;
                dashTime = dashTime - 2;
            }
        }
    }

    void ControlSpeed()
    {
        
            moveSpeed = Mathf.Lerp(moveSpeed, walkSpeed, acceleration * Time.deltaTime);
        
    }

    void ControlDrag()
    {
        if (isGrounded)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = airDrag;
        }
    }

    private void FixedUpdate()
    {
        

        //Vector3 horizontalMove = rb.velocity;
        //horizontalMove.y = 0;
        //float distance = horizontalMove.magnitude * Time.fixedDeltaTime;
        //horizontalMove.Normalize();
        //RaycastHit hit;

        //if(rb.SweepTest(horizontalMove, out hit, distance * 2))
        //{
        //    rb.velocity = new Vector3(0, rb.velocity.y, 0);
        //}

        MovePlayer();
    }

    void MovePlayer()
    {
        if (isGrounded && !OnSlope())
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * movementMultiplier, ForceMode.Acceleration);
        }
        else if (isGrounded && OnSlope())
        {
            rb.AddForce(slopeMoveDirection.normalized * moveSpeed * movementMultiplier, ForceMode.Acceleration);
        }
        else if (!isGrounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * movementMultiplier * airMultiplier, ForceMode.Acceleration);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "projectile3")
        {
            health = health - 1;//some damage value
            if(health <= 0)
            {
                Dead();
            }
        }
    }
    void Dead()
    {

    }
}
