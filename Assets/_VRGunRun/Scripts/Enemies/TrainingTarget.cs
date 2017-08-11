using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainingTarget : MonoBehaviour
{
    [SerializeField]
    private Transform activeTransform, hitTransform, targetMeshTransform, collidersTransform;
    [SerializeField]
    private Transform targetPositionA, targetPositionB;

    public bool GotHit = false;

    public int BaseRewardPoints = 10;
    public int TotalRewardPoints;
    public float TargetRange = 10f;

    public float TimeToRecover = 5f; // seconds
    public float RecoverSpeed = 1f;
    private float elapsedTimeSinceHit, reverseTime;
    private TrainingScoreManager trainingScoreManager;

    private void Awake()
    {
        trainingScoreManager = FindObjectOfType<TrainingScoreManager>();
    }


    private void Update()
    {
        if (GotHit)
        {
            elapsedTimeSinceHit += Time.deltaTime;
            targetMeshTransform.rotation = Quaternion.Lerp(activeTransform.rotation, hitTransform.rotation, elapsedTimeSinceHit * RecoverSpeed);
            collidersTransform.rotation = Quaternion.Lerp(activeTransform.rotation, hitTransform.rotation, elapsedTimeSinceHit * RecoverSpeed);

            if (elapsedTimeSinceHit > TimeToRecover)
            {
                GotHit = false;
                reverseTime = 0;
            }
        }
        else
        {
            reverseTime += Time.deltaTime;
            targetMeshTransform.rotation = Quaternion.Lerp(hitTransform.rotation, activeTransform.rotation, reverseTime * RecoverSpeed);
            collidersTransform.rotation = Quaternion.Lerp(hitTransform.rotation, activeTransform.rotation, reverseTime * RecoverSpeed);
        }

    }

    public void GetsHit(int points)
    {
        elapsedTimeSinceHit = 0;
        GotHit = true;

        trainingScoreManager.RegisterPoints(TotalRewardPoints = BaseRewardPoints + points, TargetRange);
    }
}
