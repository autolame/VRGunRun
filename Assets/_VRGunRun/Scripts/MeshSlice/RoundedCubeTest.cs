using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class RoundedCubeTest : MonoBehaviour
{
    public int SizeX, SizeY, SizeZ;
    public int Roundness;
    private Vector3[] vertices;
    private Vector3[] normals;
    private Mesh mesh;

    private void Awake()
    {
        StartCoroutine(Generate());
    }

    private IEnumerator Generate()
    {
        WaitForSeconds wait = new WaitForSeconds(1 / 1000);

        GetComponent<MeshFilter>().mesh = mesh = new Mesh();
        mesh.name = "Procedural Cube";

        CreateVerts();
        CreateTris();

        yield return wait;
    }


    void CreateVerts()
    {
        int cornerVerts = 8;
        int edgeVerts = (SizeX + SizeY + SizeZ - 3) * 4;
        int faceVerts = (
            (SizeX - 1) * (SizeY - 1) +
            (SizeX - 1) * (SizeZ - 1) +
            (SizeY - 1) * (SizeZ - 1)) * 2;
        vertices = new Vector3[cornerVerts + edgeVerts + faceVerts];
        normals = new Vector3[vertices.Length];

        int vert = 0;
        for (int y = 0; y <= SizeY; y++)
        {
            for (int x = 0; x <= SizeX; x++)
            {
                SetVertex(vert++, x, y, 0);
            }
            for (int z = 1; z <= SizeZ; z++)
            {
                SetVertex(vert++, SizeX, y, z);
            }
            for (int x = SizeX - 1; x >= 0; x--)
            {
                SetVertex(vert++, x, y, SizeZ);
            }
            for (int z = SizeZ - 1; z > 0; z--)
            {
                SetVertex(vert++, 0, y, z);
            }
        }

        for (int z = 1; z < SizeZ; z++)
        {
            for (int x = 1; x < SizeX; x++)
            {
                SetVertex(vert++, x, SizeY, z);
            }
        }
        for (int z = 1; z < SizeZ; z++)
        {
            for (int x = 1; x < SizeX; x++)
            {
                SetVertex(vert++, x, 0, z);
            }
        }

        mesh.vertices = vertices;
        mesh.normals = normals;
    }


    private void CreateTris()
    {
        int[] trisZ = new int[(SizeX * SizeY) * 12];
        int[] trisX = new int[(SizeY * SizeZ) * 12];
        int[] trisY = new int[(SizeX * SizeZ) * 12];

        int quads = (SizeX * SizeY + SizeX * SizeZ + SizeY * SizeZ) * 2;
        int[] tris = new int[quads * 6];
        int ring = (SizeX + SizeZ) * 2;
        int triZ = 0, triX = 0, triY = 0, tri = 0, vert = 0;

        for (int y = 0; y < SizeY; y++, vert++)
        {
            for (int quad = 0; quad < ring - 1; quad++, vert++)
            {
                tri = SetQuad(tris, tri, vert, vert + 1, vert + ring, vert + ring + 1);
            }
            tri = SetQuad(tris, tri, vert, vert - ring + 1, vert + ring, vert + 1);
        }

        tri = CreateTopFace(tris, tri, ring);
        tri = CreateBottomFace(tris, tri, ring);

        mesh.triangles = tris;
    }

    private int CreateTopFace(int[] tris, int tri, int ring)
    {
        int vert = ring * SizeY;
        for (int x = 0; x < SizeX - 1; x++, vert++)
        {
            tri = SetQuad(tris, tri, vert, vert + 1, vert + ring - 1, vert + ring);
        }
        tri = SetQuad(tris, tri, vert, vert + 1, vert + ring - 1, vert + 2);

        int vertMin = ring * (SizeY + 1) - 1;
        int vertMid = vertMin + 1;
        int vertMax = vert + 2;

        for (int z = 1; z < SizeZ - 1; z++, vertMin--, vertMid++, vertMax++)
        {
            tri = SetQuad(tris, tri, vertMin, vertMid, vertMin - 1, vertMid + SizeX - 1);

            for (int x = 1; x < SizeX - 1; x++, vertMid++)
            {
                tri = SetQuad(tris, tri, vertMid, vertMid + 1, vertMid + SizeX - 1, vertMid + SizeX);
            }
            tri = SetQuad(tris, tri, vertMid, vertMax, vertMid + SizeX - 1, vertMax + 1);
        }

        int vertTop = vertMin - 2;
        tri = SetQuad(tris, tri, vertMin, vertMid, vertTop + 1, vertTop);
        for (int x = 1; x < SizeX - 1; x++, vertTop--, vertMid++)
        {
            tri = SetQuad(tris, tri, vertMid, vertMid + 1, vertTop, vertTop - 1);
        }
        tri = SetQuad(tris, tri, vertMid, vertTop - 2, vertTop, vertTop - 1);

        return tri;
    }
    //private int CreateBottomFace(int[] triangles, int t, int ring)
    //{
    //    int v = 1;
    //    int vMid = vertices.Length - (SizeX - 1) * (SizeZ - 1);
    //    t = SetQuad(triangles, t, ring - 1, vMid, 0, 1);
    //    for (int x = 1; x < SizeX - 1; x++, v++, vMid++)
    //    {
    //        t = SetQuad(triangles, t, vMid, vMid + 1, v, v + 1);
    //    }
    //    t = SetQuad(triangles, t, vMid, v + 2, v, v + 1);

    //    int vMin = ring - 2;
    //    vMid -= SizeX - 2;
    //    int vMax = v + 2;

    //    for (int z = 1; z < SizeZ - 1; z++, vMin--, vMid++, vMax++)
    //    {
    //        t = SetQuad(triangles, t, vMin, vMid + SizeX - 1, vMin + 1, vMid);
    //        for (int x = 1; x < SizeX - 1; x++, vMid++)
    //        {
    //            t = SetQuad(
    //                triangles, t,
    //                vMid + SizeX - 1, vMid + SizeX, vMid, vMid + 1);
    //        }
    //        t = SetQuad(triangles, t, vMid + SizeX - 1, vMax + 1, vMid, vMax);
    //    }

    //    int vTop = vMin - 1;
    //    t = SetQuad(triangles, t, vTop + 1, vTop, vTop + 2, vMid);
    //    for (int x = 1; x < SizeX - 1; x++, vTop--, vMid++)
    //    {
    //        t = SetQuad(triangles, t, vTop, vTop - 1, vMid, vMid + 1);
    //    }
    //    t = SetQuad(triangles, t, vTop, vTop - 1, vMid, vTop - 2);

    //    return t;
    //}

    private int CreateBottomFace(int[] tris, int tri, int ring)
    {
        int vert = 1;
        int vertMid = vertices.Length - (SizeX - 1) * (SizeZ - 1);
        tri = SetQuad(tris, tri, ring - 1, vertMid, 0, 1);
        for (int x = 1; x < SizeX - 1; x++, vert++, vertMid++)
        {
            tri = SetQuad(tris, tri, vertMid, vertMid + 1, vert, vert + 1);
        }
        tri = SetQuad(tris, tri, vertMid, vert + 2, vert, vert + 1);

        int vertMin = ring - 2;
        vertMid -= SizeX - 2;
        int vertMax = vert + 2;

        for (int z = 1; z < SizeZ - 1; z++, vertMin--, vertMid++, vertMax++)
        {
            tri = SetQuad(tris, tri, vertMin, vertMid + SizeX - 1, vertMin + 1, vertMid);
            for (int x = 1; x < SizeX - 1; x++, vertMid++)
            {
                tri = SetQuad(tris, tri, vertMid + SizeX - 1, vertMid + SizeX, vertMid, vertMid + 1);
            }
            tri = SetQuad(tris, tri, vertMid + SizeX - 1, vertMax + 1, vertMid, vertMax);
        }

        int vertTop = vertMin - 1;
        tri = SetQuad(tris, tri, vertTop + 1, vertTop, vertTop + 2, vertMid);
        for (int x = 1; x < SizeX - 1; x++, vertTop--, vertMid++)
        {
            tri = SetQuad(tris, tri, vertTop, vertTop - 1, vertMid, vertMid + 1);
        }

        tri = SetQuad(tris, tri, vertTop, vertTop - 1, vertMid, vertTop - 2);

        return tri;
    }
    private static int SetQuad(int[] tris, int i, int vert00, int vert10, int vert01, int vert11)
    {
        tris[i] = vert00;
        tris[i + 1] = tris[i + 4] = vert01;
        tris[i + 2] = tris[i + 3] = vert10;
        tris[i + 5] = vert11;
        return i + 6;
    }

    private void SetVertex(int i, int x, int y, int z)
    {
        Vector3 inner = vertices[i] = new Vector3(x, y, z);

        if (x < Roundness)
        {
            inner.x = Roundness;
        }
        else if (x > SizeX - Roundness)
        {
            inner.x = SizeX - Roundness;
        }

        if (y < Roundness)
        {
            inner.y = Roundness;
        }
        else if (y > SizeY - Roundness)
        {
            inner.y = SizeY - Roundness;
        }

        if (z < Roundness)
        {
            inner.z = Roundness;
        }
        else if (z > SizeZ - Roundness)
        {
            inner.z = SizeZ - Roundness;
        }

        normals[i] = (vertices[i] - inner).normalized;
        vertices[i] = inner + normals[i] * Roundness;
    }

    private void OnDrawGizmos()
    {
        if (vertices == null)
        {
            return;
        }

        for (int i = 0; i < vertices.Length; i++)
        {
            //Gizmos.color = Color.white;
            //Gizmos.DrawSphere(vertices[i], 0.1f);
            Gizmos.color = Color.red;
            Gizmos.DrawRay(vertices[i], normals[i]);
        }
    }
}
