using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshDeformerInputTest : MonoBehaviour
{
    public float Force = 10f;
    public float ForceOffset = 0.1f;

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            HandleInput();
        }
    }

    private void HandleInput()
    {
        Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;

        if (Physics.Raycast(inputRay, out hit))
        {
            MeshDeformerTest deformer = hit.collider.GetComponent<MeshDeformerTest>();
            if (deformer)
            {
                Vector3 point = hit.point;
                point += hit.normal * ForceOffset;
                deformer.AddDeformingForce(point, Force);
            }
        }
    }
}

