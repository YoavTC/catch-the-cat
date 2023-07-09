using System;
using System.Collections;
using UnityEngine;

public class Lever : MonoBehaviour
{
    //Button
    private GameObject lever;
    private GameObject moveAble;

    //Positions
    private Rigidbody moveAbleRigidbody;
    private Vector3 posA;
    private Vector3 posB;
    [SerializeField] private float transitionSpeed;
    [SerializeField] private float transitionCutoff;

    [Header("Lever")] 
    [SerializeField] private bool allowedToToggle = true;
    [SerializeField] private bool canToggle;
    [SerializeField] private bool isActivated;
    [SerializeField] private float cooldown = 0.8f;

    private void Start()
    {
        moveAble = transform.parent.GetChild(1).transform.gameObject;
        moveAbleRigidbody = moveAble.GetComponent<Rigidbody>();

        posA = moveAble.transform.position;
        posB = moveAble.transform.GetChild(0).position;
    }

    private IEnumerator OnFlipLeverOn()
    {
        float timer = 0f;
        while (moveAbleRigidbody.transform.position != posB && timer < transitionCutoff)
        {
            Debug.Log("Moving to B!");
            moveAbleRigidbody.transform.position = Vector3.Lerp(moveAble.transform.position, posB, transitionSpeed * Time.deltaTime);
            yield return new WaitForEndOfFrame();

            timer += Time.deltaTime;

            if (timer >= transitionCutoff)
            {
                moveAbleRigidbody.transform.position = posB;
                Debug.Log("Breaking!");
                yield break;
            }
        }
    }
    
    private IEnumerator OnFlipLeverOff()
    {
        float timer = 0f;
        while (moveAbleRigidbody.transform.position != posA && timer < transitionCutoff)
        {
            Debug.Log("Moving to A!");
            moveAbleRigidbody.transform.position = Vector3.Lerp(moveAbleRigidbody.transform.position, posA, transitionSpeed * Time.deltaTime);
            yield return new WaitForEndOfFrame();

            timer += Time.deltaTime;

            if (timer >= transitionCutoff)
            {
                moveAbleRigidbody.transform.position = posA;
                Debug.Log("Breaking!");
                yield break;
            }
        }
    }
    
    private void Update()
    {
        if (canToggle && allowedToToggle && Input.GetButton("Interact"))
        {
            if (isActivated) 
            {   
                //TOGGLE THE LEVER OFF!
                StartCoroutine(OnFlipLeverOff());
            }
            else            
            {   
                //TOGGLE THE LEVER ON!
                StartCoroutine(OnFlipLeverOn());
            }

            isActivated = !isActivated;
            StartCoroutine(ToggleCooldown());
        }
    }

    IEnumerator ToggleCooldown()
    {
        allowedToToggle = false;
        yield return new WaitForSeconds(cooldown);
        allowedToToggle = true;
    }
    
    #region Triggers

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) canToggle = true;
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) canToggle = false;
    }

    #endregion
}
