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

    public Player player;

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
    // based on this formula http://en.wikipedia.org/wiki/Trajectory_of_a_projectile#Angle_required_to_hit_coordinate_.28x.2Cy.29
    bool FindLaunchVector(Vector3 startPosition, Vector3 targetPosition, float launchSpeed, bool useHighArc, out Vector3 foundLaunchVector)
    {
        foundLaunchVector = Vector3.zero;

        Vector3 flightVector = targetPosition - startPosition;
        Vector3 flightVectorHorizontal = new Vector3(flightVector.x, 0, flightVector.z);
        Vector3 flightDirectionHorizontal = flightVectorHorizontal.normalized;

        float flightHorizontalMag = flightVectorHorizontal.magnitude;

        float flightVertical = flightVector.z;

        float launchSpeedSQ = launchSpeed * launchSpeed;

        float gravity = -Physics.gravity.z;

        // v^4 - g*(g*x^2 + 2*y*v^2)
        float inTheSQ = launchSpeedSQ * launchSpeedSQ - gravity * ((gravity * flightHorizontalMag * flightHorizontalMag) + (2.0f * flightVertical * launchSpeedSQ));
        // no solution of sqrt < 0
        if (inTheSQ < 0.0f)
        {
            return false;
        }

        // if we got here, there are 2 solutions: one high-angle and one low-angle.
        float sqrtPortion = Mathf.Sqrt(inTheSQ);

        // addition [+] solition
        float solutionAdd = (launchSpeedSQ + sqrtPortion) / (gravity * flightHorizontalMag);
        // subtraction [-] solution
        float solutionSub = (launchSpeedSQ - sqrtPortion) / (gravity * flightHorizontalMag);

        // magnitude of the horizontal direction = sqrt( TossSpeedSq / (TanSolutionAngle^2 + 1) );
        float horSQMagAddSol = launchSpeedSQ / (solutionAdd * solutionAdd + 1.0f);
        float horSQMagSubSol = launchSpeedSQ / (solutionSub * solutionSub + 1.0f);

        bool foundValidLaunchVector = false;

        // choose low or high arc based on bool useHighArc
        float chosenHorMagSQ = useHighArc ?
            Mathf.Min(horSQMagAddSol, horSQMagSubSol) : Mathf.Max(horSQMagAddSol, horSQMagSubSol);
        float verticalSign = useHighArc ?
            (horSQMagAddSol < horSQMagSubSol) ? Mathf.Sign(solutionAdd) : Mathf.Sign(solutionSub) :
            (horSQMagAddSol > horSQMagSubSol) ? Mathf.Sign(solutionAdd) : Mathf.Sign(solutionSub);

        // warpping up calculations
        float horizontalMag = Mathf.Sqrt(chosenHorMagSQ);
        float verticalMag = Mathf.Sqrt(launchSpeedSQ - chosenHorMagSQ);

        foundLaunchVector = (flightDirectionHorizontal * horizontalMag) + (Vector3.up * verticalMag * verticalSign);
        foundValidLaunchVector = true;

        return foundValidLaunchVector;
    }
}


