using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPlane : MonoBehaviour
{

    [SerializeField] float HitPoints = 1000;
    [SerializeField] ParticleFX explosionFX;

    public Transform TargetTransform;
    public float MoveSpeed = 1;

    public EnemySpawn droneSpawn;
    public Valve.VR.InteractionSystem.Player player;

    private void Awake()
    {
        player = FindObjectOfType<Valve.VR.InteractionSystem.Player>();
        droneSpawn = GetComponent<EnemySpawn>();
        droneSpawn.Goal = player.hmdTransforms[0].GetComponent<EnemyGoal>();
    }
    public void MoveTowardsTarget(Vector3 targetPosition)
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, MoveSpeed);
    }


    private void Update()
    {
        transform.LookAt(TargetTransform);

        if (CheckIfAlive())
        {
            MoveTowardsTarget(TargetTransform.position);
        }
        else
        {
            explosionFX.SpawnAt(transform.position, 5f);
            FindObjectOfType<SlomoManager>().StartSlomoFor(10);
            Destroy(gameObject);
        }

        if (transform.position == TargetTransform.position)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<GunAmmoBullet>())
        {
            HitPoints -= collision.gameObject.GetComponent<Rigidbody>().velocity.magnitude;
        }
    }

    bool CheckIfAlive()
    {
        if (HitPoints > 0)
        { return true; }
        else
        { return false; }
    }
}
