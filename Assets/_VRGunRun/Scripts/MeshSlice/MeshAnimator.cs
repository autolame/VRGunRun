using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshAnimator : MonoBehaviour
{
    private Mesh mesh;

    public float Speed = 1f;



    private void Awake()
    {
        mesh = GetComponent<MeshFilter>().mesh;
    }

    private void Update()
    {
        var verts = mesh.vertices;


        for (int i = 0; i < verts.Length; i++)
        {
            var modI = i % 2;

            if (modI != 0)
            {
                verts[i].y += Mathf.Sin(Speed * Time.time) / 10;
                verts[i].x += Mathf.Sin(Speed * Time.time) / 10;
            }
            else
            {
                verts[i].z += Mathf.Sin(Speed * Time.time) / 10;
                verts[i].x -= Mathf.Sin(Speed * Time.time) / 10;
            }
        }

        mesh.vertices = verts;
    }



}
