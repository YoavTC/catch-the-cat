using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tail : MonoBehaviour
{
    private LineRenderer line;
    private List<GameObject> joints = new List<GameObject>();

    private void Start()
    {
        line = GetComponent<LineRenderer>();
        for (int i = 0; i < transform.childCount; i++)
        {
            joints.Add(transform.GetChild(i).gameObject);
            Debug.Log(joints[i]);
        }
        line.positionCount = joints.Count;
        Debug.Log(line);
    }

    private void Update()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            line.SetPosition(i, joints[i].transform.position);
        }
    }
}
