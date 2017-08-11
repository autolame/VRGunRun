using UnityEngine;
using System.Collections;

public class ExampleClass : MonoBehaviour
{
    public Plane groundPlane;
    public Transform markerObject;
    void Update()
    {
        groundPlane = new Plane(transform.position, transform.forward, transform.right);

        var planeNormal = Vector3.Cross(transform.forward, transform.right).normalized;

        groundPlane.SetNormalAndPosition(planeNormal, transform.position);


        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float rayDistance;
            if (groundPlane.Raycast(ray, out rayDistance))
                markerObject.position = ray.GetPoint(rayDistance);

        }
    }
}