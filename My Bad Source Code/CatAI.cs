using System;
using System.Collections;
using UnityEngine;

public class CatAI : MonoBehaviour
{
    [SerializeField] private GameObject playerObject;

    private Vector3 fleeDirection;
    [SerializeField] float catSpeed = 7f;
    [SerializeField] private AnimationCurve speedCurve;
    [SerializeField] float range;
    [SerializeField] private float portalRange;
    
    private Rigidbody rb;
    private float temporaryYvelocity;

    //Jumping
    [SerializeField] LayerMask groundMask;
    float groundDistance = 0.4f;
    [SerializeField] private Transform groundCheck;
    private bool isGrounded;

    private bool playerAboveCat;
    private bool isFacingRight = false;
    
    //End Conditioning
    [SerializeField] bool isGettingCaught;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        isGettingCaught = false;
    }

    private void Update()
    {
        //calculate movement speed
        catSpeed = speedCurve.Evaluate(Vector3.Distance(transform.position, playerObject.transform.position)) * 100;
        
        float playerX = playerObject.transform.position.x;
        float catX = gameObject.transform.position.x;

        if (isGrounded)
        {
            double maxCatX = catX + 1;
            double minCatX = catX - 1;
            
            Debug.DrawLine(transform.position, new Vector3((float)minCatX, 0, 0), Color.green);
            Debug.DrawLine(transform.position, new Vector3((float)maxCatX, 0, 0), Color.magenta);

            if (playerX > minCatX && playerX < maxCatX)
            {
                playerAboveCat = true;
            }
        }
        
        //Flipping
        if (isFacingRight == false && rb.velocity.x > 0)
        {
            Flip();
        } else if (isFacingRight && rb.velocity.x < 0)
        {
            Flip();
        }
    }


    private void FixedUpdate()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        
        //Should Flee?
        if (Vector3.Distance(playerObject.transform.position, gameObject.transform.position) < range)
        {
            float playerX = playerObject.transform.position.x;
            float catX = gameObject.transform.position.x;
            
            //Save Y velocity to not effect gravity
            temporaryYvelocity = rb.velocity.y;
            
            if (playerAboveCat) //Side flee
            {
                //decide weather to move the cat left or right based on the player's X position
                fleeDirection = playerX > catX ? Vector3.left : Vector3.right;
            } 
            else { //Angle flee
                fleeDirection = transform.position - playerObject.transform.position;
            }
            
            fleeDirection.Normalize();
            Vector3 fleeVelocity = fleeDirection * catSpeed * Time.fixedDeltaTime;
            
            //Applying the saved Y velocity to not effect gravity
            fleeVelocity.y = temporaryYvelocity;
            rb.velocity = fleeVelocity; 
        }
        //Slowly smoothly slow down the cat
        else if (rb.velocity != Vector3.zero)
        {
            temporaryYvelocity = rb.velocity.y;
            rb.velocity = Vector3.Lerp(rb.velocity, Vector3.zero, 5f * Time.fixedDeltaTime);
            rb.velocity = new Vector3(rb.velocity.x, temporaryYvelocity, rb.velocity.z);
        }
    }
    
    //End game check
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("COL: " + other.gameObject.name);
        if (other.transform.CompareTag("Player"))
        {
            isGettingCaught = true;
            StartCoroutine(CollfalseDetection());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) isGettingCaught = false;
    }

    void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 temp = transform.localScale;
        temp.y *= -1;
        transform.localScale = temp;
    }

    public IEnumerator AfterTeleport(Portal portal, Portal previousPortal)
    {
        yield return new WaitForSeconds(0.15f);
        Debug.Log("DIS1: " + Vector3.Distance(transform.position, portal.transform.position));
        Debug.Log("DIS2: " + Vector3.Distance(previousPortal.transform.position, playerObject.transform.position));
        if (Vector3.Distance(transform.position, portal.transform.position) < portalRange && Vector3.Distance(previousPortal.transform.position, playerObject.transform.position) < portalRange)
        {
            float directionTowardsPlayer = transform.position.x > playerObject.transform.position.x ? -1 : 1;
            rb.velocity = new Vector3(directionTowardsPlayer * (catSpeed * 0.5f), rb.velocity.y, rb.velocity.z);
        }
    }

    IEnumerator CollfalseDetection()
    {
        yield return new WaitForSeconds(0.06f);
        if (isGettingCaught) GameManager.Intance.LoadNextLevel();
    }
}
