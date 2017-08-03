using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class Grid : MonoBehaviour
{

    public int SizeX, SizeY;
    private Vector3[] vertices;
    private Mesh mesh;

    private void Awake()
    {
        StartCoroutine(Generate());
    }



    private IEnumerator Generate()
    {
        WaitForSeconds wait = new WaitForSeconds(1 / 1000);

        GetComponent<MeshFilter>().mesh = mesh = new Mesh();
        mesh.name = "Procedural Grid";

        vertices = new Vector3[(SizeX + 1) * (SizeY + 1)];
        Vector2[] uv = new Vector2[vertices.Length];
        Vector4[] tangents = new Vector4[vertices.Length];
        Vector4 tangent = new Vector4(1f, 0f, 0f, -1f);

        for (int i = 0, y = 0; y <= SizeY; y++)
        {
            for (int x = 0; x <= SizeX; x++, i++)
            {
                vertices[i] = new Vector3(x, y);
                uv[i] = new Vector2((float)x / SizeX, (float)y / SizeY);
                tangents[i] = tangent;
            }
        }

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.tangents = tangents;

        int[] triangles = new int[SizeX * SizeY * 6];
        for (int triI = 0, vertI = 0, y = 0; y < SizeY; y++, vertI++)
        {
            for (int x = 0; x < SizeX; x++, triI += 6, vertI++)
            {
                triangles[triI] = vertI;
                triangles[triI + 1] = triangles[triI + 4] = vertI + SizeX + 1;
                triangles[triI + 2] = triangles[triI + 3] = vertI + 1;
                triangles[triI + 5] = vertI + SizeX + 2;
                mesh.triangles = triangles;
                yield return wait;
            }
            mesh.RecalculateNormals();
        }
    }

    //private void OnDrawGizmos()
    //{
    //    if (vertices == null)
    //    {
    //        return;
    //    }

    //    Gizmos.color = Color.white;
    //    foreach (var vert in vertices)
    //    {
    //        Gizmos.DrawSphere(vert, 0.1f);
    //    }
    //}

}
