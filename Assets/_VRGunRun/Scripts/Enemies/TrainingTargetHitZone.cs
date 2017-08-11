using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainingTargetHitZone : MonoBehaviour
{
    [SerializeField]
    private TrainingTarget trainingTarget;
    public int RewardPoints = 10;




    private void OnCollisionEnter(Collision collision)
    {
        if (!trainingTarget)
        {
            return;
        }
        else
        {
            Debug.LogWarning("hit on target");
            if (collision.gameObject.GetComponent<GunAmmoBullet>())
            {
                trainingTarget.GetsHit(RewardPoints);
            }
        }

    }
}
