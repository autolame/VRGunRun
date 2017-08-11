using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class MeshDeformerTest : MonoBehaviour
{
    Mesh deformingMesh;
    Vector3[] originalVerts, displacedVerts;
    Vector3[] vertVelocities;

    public float SpringForce = 20f;
    public float Damping = 5f;
    float uniformScale = 1f;

    private void Start()
    {
        deformingMesh = GetComponent<MeshFilter>().mesh;
        originalVerts = deformingMesh.vertices;
        displacedVerts = new Vector3[originalVerts.Length];

        for (int i = 0; i < originalVerts.Length; i++)
        {
            displacedVerts[i] = originalVerts[i];
        }

        vertVelocities = new Vector3[originalVerts.Length];
    }

    private void Update()
    {
        uniformScale = transform.localScale.x;
        for (int i = 0; i < displacedVerts.Length; i++)
        {
            UpdateVert(i);
        }
        deformingMesh.vertices = displacedVerts;
        deformingMesh.RecalculateNormals();
    }

    private void UpdateVert(int i)
    {
        Vector3 velocity = vertVelocities[i];
        Vector3 displacement = displacedVerts[i] - originalVerts[i];
        displacement *= uniformScale;
        velocity -= displacement * SpringForce * Time.deltaTime;
        velocity *= 1f - Damping * Time.deltaTime;
        vertVelocities[i] = velocity;
        displacedVerts[i] += velocity * (Time.deltaTime / uniformScale);
    }

    public void AddDeformingForce(Vector3 point, float force)
    {
        point = transform.InverseTransformPoint(point);
        for (int i = 0; i < displacedVerts.Length; i++)
        {
            AddForceToVert(i, point, force);
        }

    }

    void AddForceToVert(int i, Vector3 point, float force)
    {
        Vector3 pointToVert = displacedVerts[i] - point;
        pointToVert *= uniformScale;
        float attenForce = force / (1f + pointToVert.sqrMagnitude);
        float velocity = attenForce * Time.deltaTime;
        vertVelocities[i] += pointToVert.normalized * velocity;
    }
}
