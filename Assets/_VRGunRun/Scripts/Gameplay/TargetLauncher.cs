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
    [SerializeField] float maxLaunchForce = 50; // meter/sec
    [SerializeField] float minLaunchForce = 10; // meter/sec
    [SerializeField] float nextLaunchForce = 20;
    [SerializeField] bool useHighArc = false;

    public float StartLaunchRate = 1;

    public bool findLaunchVector;

    float red, green, blue;

    public Valve.VR.InteractionSystem.Player VRPlayer;

    float elapsedTime;
    public float ElapsedTime
    {
        get { return elapsedTime; }
        set { elapsedTime = value; }
    }

    private void Update()
    {
        ElapsedTime += Time.deltaTime;
        red = Random.Range(0, 2);
        green = Random.Range(0, 2);
        blue = Random.Range(0, 2);

        nextLaunchForce = Random.Range(minLaunchForce, maxLaunchForce);

        if (ElapsedTime >= StartLaunchRate)
        {
            ElapsedTime = 0;
            //LaunchTarget();
            LaunchTargetTowardsPlayer();
            StartLaunchRate = Random.Range(minlaunchRate, maxlaunchRate);
        }
    }
    void LaunchTarget()
    {
        TargetDummy newTarget = Instantiate(targetDummy);
        newTarget.gameObject.SetActive(true);
        newTarget.transform.position = transform.position;

        newTarget.gameObject.GetComponent<Rigidbody>().velocity = transform.up * Random.Range(minLaunchForce, maxLaunchForce);
        newTarget.gameObject.GetComponent<Rigidbody>().angularVelocity = new Vector3(Random.Range(-maxLaunchForce, maxLaunchForce), Random.Range(-maxLaunchForce, maxLaunchForce), Random.Range(-maxLaunchForce, maxLaunchForce));
        newTarget.gameObject.GetComponent<MeshRenderer>().material.color = new Color(red, green, blue);
        Destroy(newTarget.gameObject, 30f);
    }
    void LaunchTargetTowardsPlayer()
    {
        Vector3 playerPosition = VRPlayer.hmdTransform.position;

        Vector3 launchPosition = transform.position;
        Vector3 launchVector = Vector3.zero;

        findLaunchVector = FindLaunchVector(launchPosition, playerPosition, nextLaunchForce, useHighArc, out launchVector);

        if (findLaunchVector)
        {
            TargetDummy newTarget = Instantiate(targetDummy);
            newTarget.transform.position = transform.position;
            newTarget.gameObject.SetActive(true);

            newTarget.gameObject.GetComponent<Rigidbody>().velocity = launchVector;
            newTarget.gameObject.GetComponent<Rigidbody>().angularVelocity = new Vector3(Random.Range(-maxLaunchForce, maxLaunchForce), Random.Range(-maxLaunchForce, maxLaunchForce), Random.Range(-maxLaunchForce, maxLaunchForce));
            newTarget.gameObject.GetComponent<MeshRenderer>().material.color = new Color(red, green, blue);
            Destroy(newTarget.gameObject, 30f);
        }
    }

    //launchVector formula http://en.wikipedia.org/wiki/Trajectory_of_a_projectile#Angle_required_to_hit_coordinate_.28x.2Cy.29
    bool FindLaunchVector(Vector3 launchPosition, Vector3 targetPosition, float launchForce, bool useHighArc, out Vector3 foundLaunchVector)
    {
        foundLaunchVector = Vector3.zero;

        Vector3 flightVector = targetPosition - launchPosition;
        Vector3 horizontalFlightVector = new Vector3(flightVector.x, 0, flightVector.z);

        Vector3 horizontalFlightDirection = horizontalFlightVector.normalized;
        float vertitalFlightMag = horizontalFlightVector.magnitude;

        float verticalFlight = flightVector.y;

        float launchForceSQ = Mathf.Pow(launchForce, 2);

        float gravity = -Physics.gravity.y;

        // v^4 - g*(g*x^2 + 2*y*v^2)
        float inSqrt = Mathf.Pow(launchForceSQ, 2) - gravity * ((gravity * Mathf.Pow(vertitalFlightMag, 2)) + (2.0f * verticalFlight * launchForceSQ));
        // no solution because sqrt < 0
        if (inSqrt < 0.0f)
        {
            return false;
        }
        // two solutions (quadratic formula) low arc and high arc
        float sqrt = Mathf.Sqrt(inSqrt);

        // [+] solution
        float TanSolutionA = (launchForceSQ + sqrt) / (gravity * vertitalFlightMag);
        // [-] solution
        float TanSolutionB = (launchForceSQ - sqrt) / (gravity * vertitalFlightMag);

        // horizontal magnitude = sqrt( TossSpeedSq / (TanSolutionAngle^2 + 1) );
        float horizontalSQMagA = launchForceSQ / (Mathf.Pow(TanSolutionA, 2) + 1.0f);
        float horizontalSQMagB = launchForceSQ / (Mathf.Pow(TanSolutionB, 2) + 1.0f);

        bool launchVectorFound = false;

        // decide on chosen arc
        float chosenHorizontalMagnitudeSQ = useHighArc ? Mathf.Min(horizontalSQMagA, horizontalSQMagB) : Mathf.Max(horizontalSQMagA, horizontalSQMagB);
        float verticalSign = useHighArc ?
                           (horizontalSQMagA < horizontalSQMagB) ? Mathf.Sign(TanSolutionA) : Mathf.Sign(TanSolutionB) :
                           (horizontalSQMagA > horizontalSQMagB) ? Mathf.Sign(TanSolutionA) : Mathf.Sign(TanSolutionB);

        // wrapping up calculations
        float MagXY = Mathf.Sqrt(chosenHorizontalMagnitudeSQ);
        float MagZ = Mathf.Sqrt(launchForceSQ - chosenHorizontalMagnitudeSQ);       // pythagorean

        foundLaunchVector = (horizontalFlightDirection * MagXY) + (Vector3.up * MagZ * verticalSign);
        launchVectorFound = true;

        return launchVectorFound;
    }
}

