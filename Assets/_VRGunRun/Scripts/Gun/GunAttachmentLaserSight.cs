//======= Copyright (c) Viet Kien Nguyen, All rights reserved. ===============
//
// Purpose: laser sight for the gun
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class GunAttachmentLaserSight : MonoBehaviour
{
    LineRenderer laserSightRenderer;
    [SerializeField] Transform light;

    private void Awake()
    {
        laserSightRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        RaycastHit laserHit;
        if (Physics.Raycast(transform.position, transform.forward, out laserHit))
        {
            laserSightRenderer.useWorldSpace = true;
            laserSightRenderer.SetPosition(0, transform.position);
            laserSightRenderer.SetPosition(1, laserHit.point);

            light.position = laserHit.point - (transform.forward / 1000);
            light.gameObject.SetActive(true);
        }
        else
        {
            laserSightRenderer.useWorldSpace = false;
            laserSightRenderer.SetPosition(0, Vector3.zero);
            laserSightRenderer.SetPosition(1, Vector3.forward * 100);

            light.gameObject.SetActive(false);
        }
    }


}
