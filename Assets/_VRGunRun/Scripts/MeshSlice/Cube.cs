using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class Cube : MonoBehaviour
{
    public int SizeX, SizeY, SizeZ;
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

        int vert = 0;
        for (int y = 0; y <= SizeY; y++)
        {
            for (int x = 0; x <= SizeX; x++)
            {
                vertices[vert++] = new Vector3(x, y, 0);
            }
            for (int z = 1; z <= SizeZ; z++)
            {
                vertices[vert++] = new Vector3(SizeX, y, z);
            }
            for (int x = SizeX - 1; x >= 0; x--)
            {
                vertices[vert++] = new Vector3(x, y, SizeZ);
            }
            for (int z = SizeZ - 1; z > 0; z--)
            {
                vertices[vert++] = new Vector3(0, y, z);
            }
        }

        for (int z = 1; z < SizeZ; z++)
        {
            for (int x = 1; x < SizeX; x++)
            {
                vertices[vert++] = new Vector3(x, SizeY, z);
            }
        }
        for (int z = 1; z < SizeZ; z++)
        {
            for (int x = 1; x < SizeX; x++)
            {
                vertices[vert++] = new Vector3(x, 0, z);
            }
        }
        mesh.vertices = vertices;
    }


    private void CreateTris()
    {
        int quads = (SizeX * SizeY + SizeX * SizeZ + SizeY * SizeZ) * 2;
        int[] tris = new int[quads * 6];
        int ring = (SizeX + SizeZ) * 2;
        int tri = 0, vert = 0;

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
