using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BumpMine : MonoBehaviour
{
    [SerializeField] private float strength;

    private void OnTriggerEnter(Collider other)
    {
        Vector3 velocity = other.gameObject.GetComponent<Rigidbody>().velocity;
        other.GetComponent<Rigidbody>().velocity = new Vector3(velocity.x, strength, velocity.z);
    }
}
