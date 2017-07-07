//======= Copyright (c) Viet Kien Nguyen, All rights reserved. ===============
//
// Purpose: practice targets launcher
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetLauncher : MonoBehaviour
{

    [SerializeField] TargetDummy targetDummy;
    [SerializeField] float maxlaunchRate = 10; // each second
    [SerializeField] float minlaunchRate = 5; // each second
    [SerializeField] float maxLaunchForce = 50;
    [SerializeField] float minLaunchForce = 10;
    public float LaunchRate = 1;
    float elapsedTime;
    public float ElapsedTime
    {
        get { return elapsedTime; }
        set { elapsedTime = value; }
    }

    private void Update()
    {
        ElapsedTime += Time.deltaTime;

        if (ElapsedTime >= LaunchRate)
        {
            ElapsedTime = 0;
            LaunchTarget();
            LaunchRate = Random.Range(minlaunchRate, maxlaunchRate);
        }
    }
    void LaunchTarget()
    {
        TargetDummy newTarget = Instantiate(targetDummy);
        newTarget.gameObject.SetActive(true);
        newTarget.transform.position = transform.position + Vector3.up * 3;

        newTarget.gameObject.GetComponent<Rigidbody>().velocity = transform.up * Random.Range(minLaunchForce, maxLaunchForce);
        newTarget.gameObject.GetComponent<Rigidbody>().angularVelocity = new Vector3(Random.Range(-maxLaunchForce, maxLaunchForce), Random.Range(-maxLaunchForce, maxLaunchForce), Random.Range(-maxLaunchForce, maxLaunchForce));
        newTarget.gameObject.GetComponent<MeshRenderer>().material.color = new Color(Random.Range(0, 2), Random.Range(0, 2), Random.Range(0, 2));
    }
}
