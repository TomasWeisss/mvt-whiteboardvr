using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixDepthCorrect : MonoBehaviour
{
    public float maxZ;
    public Transform target;
    private Transform _child;


    private void Start()
    {
        _child = transform.GetChild(0);
    }

    void Update()
    {
        float diff = target.transform.position.z - maxZ;

        //diff = Mathf.Max(0, diff);

        _child.transform.position = transform.position - Vector3.forward * diff;
    }
}
