using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnitySampleAssets.ImageEffects;

public class SliceableMesh : MonoBehaviour
{
    public MeshSlicer Slicer;

    public Mesh thisMesh;
    public Vector3[] thisMeshVerts, thisMeshWorldVerts;

    public Mesh genLeftMesh, genRightMesh;
    public List<Vector3> leftMeshVertList = new List<Vector3>();
    public List<Vector3> rightMeshVertList = new List<Vector3>();
    public Vector3[] leftMeshVerts, rightMeshVerts;

    public Plane slicePlane;

    void Awake()
    {
        thisMesh = GetComponent<MeshFilter>().mesh;
        slicePlane = new Plane(
            //transform.InverseTransformDirection(Slicer.transform.right),
            //transform.InverseTransformPoint(Slicer.transform.position)
            Slicer.transform.right,
            Slicer.transform.position
            );
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (Slicer)
                SliceMesh();
        }
    }

    void SliceMesh()
    {
        slicePlane.SetNormalAndPosition(
            transform.InverseTransformDirection(Slicer.transform.right),
            transform.InverseTransformPoint(Slicer.transform.position)
            );


        Debug.LogWarning(
           transform.InverseTransformDirection(Slicer.transform.right));
        Debug.LogWarning(
           transform.InverseTransformPoint(Slicer.transform.position));

        thisMeshVerts = thisMesh.vertices;
        thisMeshWorldVerts = new Vector3[thisMeshVerts.Length];
        var thisMeshTris = thisMesh.triangles;

        leftMeshVertList.Clear();
        rightMeshVertList.Clear();

        foreach (var tri in thisMeshTris)
        {

        }
        for (int i = 0; i < thisMeshVerts.Length; i++)
        {
            var worldPos = transform.TransformPoint(thisMeshVerts[i]);
            thisMeshWorldVerts[i] = worldPos;
        }
        foreach (var vert in thisMeshWorldVerts)
        {
            //var dotProd = Vector3.Dot(slicePlane.normal + Slicer.transform.position, vert);
            //Debug.Log(slicePlane.normal);

            var getSide = slicePlane.GetSide(vert);
            if (getSide)
            {
                rightMeshVertList.Add(vert);
            }
            else
            {
                leftMeshVertList.Add(vert);
            }
        }

        leftMeshVerts = leftMeshVertList.ToArray();
        rightMeshVerts = rightMeshVertList.ToArray();

        // TODO assign the meshes
        Debug.Log("leftMeshVertList.Count" + leftMeshVertList.Count);
        Debug.Log("leftMeshVerts.Length" + leftMeshVerts.Length);
        Debug.Log("rightMeshVertList.Count" + rightMeshVertList.Count);
        Debug.Log("rightMeshVerts.Length" + rightMeshVerts.Length);
    }


    private void OnDrawGizmos()
    {
        //foreach (var vert in thisMeshWorldVerts)
        //{
        //    Gizmos.DrawRay(transform.position, vert);
        //}


        Gizmos.color = Color.green;

        Gizmos.DrawRay(
                   transform.InverseTransformPoint(Slicer.transform.position),
            transform.InverseTransformDirection(Slicer.transform.right)
                   );
    }
}
