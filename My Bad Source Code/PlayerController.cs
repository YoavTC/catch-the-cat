using System;
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Fields")]
    [SerializeField] private float playerSpeed;
    [SerializeField] private float jumpStrength;
    private float horizontalInput;
    
    [Header("Jumping")]
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundMask;
    float groundDistance = 0.4f;
    bool isGrounded;

    [Header("Other")]
    private Rigidbody rb;

    private bool isFacingRight = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        //Is grounded check
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        
        //Get movement
        horizontalInput = Input.GetAxis("Horizontal");
        
        //Flipping
        #region Flipping

        if (isFacingRight == false && horizontalInput > 0)
        {
            Flip();
        } else if (isFacingRight && horizontalInput < 0)
        {
            Flip();
        }

        #endregion
        
        //Get Jumping
        if (Input.GetButton("Jump") && isGrounded)
        {
            Jump();
        } 
        
    }

    void FixedUpdate()
    {
        //Apply movement
        rb.velocity = new Vector3(horizontalInput * playerSpeed * Time.fixedDeltaTime, rb.velocity.y, rb.velocity.z);
    }
    
    void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 temp = transform.localScale;
        temp.y *= -1;
        transform.localScale = temp;
    }

    void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, jumpStrength, rb.velocity.z);
    }
}
