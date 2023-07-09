using System;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] private Portal secondPortal;
    Transform leaveLocation;

    private void Start()
    {
        for (int i = 0; i < transform.parent.childCount; i++)
        {
            if (transform.parent.GetChild(i) != transform)
            {
                leaveLocation = transform.parent.GetChild(i);
            }
        }

        leaveLocation = transform.GetChild(0);
    }

    private void OnTriggerEnter(Collider other)
    {
        secondPortal.GoThroughMe(other.gameObject, this);
    }

    public void GoThroughMe(GameObject traveler, Portal previousPortal)
    {
        if (!traveler.CompareTag("Env"))
        {
            float tempZ = traveler.transform.position.z;
            Vector3 travelerPos = leaveLocation.transform.position;
        
            if (traveler.CompareTag("Cat"))
            {
                StartCoroutine(traveler.GetComponent<CatAI>().AfterTeleport(this, previousPortal));
                travelerPos = leaveLocation.transform.position;
            }
            else if (traveler.CompareTag("Player"))
            {
                travelerPos = Vector3.Lerp(leaveLocation.transform.position, transform.position, 0.6f);
            }
            

            traveler.transform.position = new Vector3(travelerPos.x, travelerPos.y, tempZ);
        }
    }

    public Portal GetSisterPortal()
    {
        return secondPortal;
    }
}
