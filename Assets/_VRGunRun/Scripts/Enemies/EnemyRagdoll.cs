using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRagdoll : MonoBehaviour
{

    public Enemy Enemy;
    //public EnemyRagdoll Ragdoll;
    public List<Transform> EnemyBoneTransforms = new List<Transform>();
    public List<Transform> RagdollBoneTransforms = new List<Transform>();


    public void PoseRagdoll()
    {
        foreach (var ragdollBone in RagdollBoneTransforms)
        {
            if (ragdollBone.GetComponent<Rigidbody>())
            {
                ragdollBone.GetComponent<Rigidbody>().isKinematic = true;
            }
            foreach (var enemyBone in EnemyBoneTransforms)
            {
                if (enemyBone.name == ragdollBone.name)
                {
                    ragdollBone.transform.position = enemyBone.transform.position;
                    ragdollBone.transform.rotation = enemyBone.transform.rotation;
                    ragdollBone.transform.localPosition = enemyBone.transform.localPosition;
                    ragdollBone.transform.localRotation = enemyBone.transform.localRotation;
                }
            }
            if (ragdollBone.GetComponent<Rigidbody>())
            {
                ragdollBone.GetComponent<Rigidbody>().isKinematic = false;
            }
        }
    }
    private void Start()
    {
        GetRagdollPose();
    }
    public void GetRagdollPose()
    {
        foreach (Transform bone in GetComponentsInChildren<Transform>())
        {
            RagdollBoneTransforms.Add(bone);
        }

        foreach (Transform bone in Enemy.GetComponentsInChildren<Transform>())
        {
            EnemyBoneTransforms.Add(bone);
        }
    }




}
