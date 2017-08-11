using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshSlicer : MonoBehaviour
{
    public Plane SlicePlane;
    public Vector3 PlaneOrigin, PlaneNormal;

    private void Update()
    {
        PlaneOrigin = transform.position;
        PlaneNormal = transform.right;

        SlicePlane = new Plane(PlaneNormal, PlaneOrigin);
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawRay(PlaneOrigin, transform.forward);
        Gizmos.DrawRay(PlaneOrigin, transform.up);
        Gizmos.DrawRay(PlaneOrigin, transform.forward + transform.up);

        Gizmos.color = Color.green;

        Gizmos.DrawRay(PlaneOrigin, PlaneNormal);
    }



}
