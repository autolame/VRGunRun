using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{

    public float StartHitPoint = 2500f;
    public List<EnemyDrone> DronesInRange = new List<EnemyDrone>();

    [SerializeField] private Transform turretHead;
    [SerializeField] private GunAmmoBullet projectile;
    [SerializeField] private Transform muzzle;
    [SerializeField] private float muzzleVelocity = 1500f;
    [SerializeField] private float fireRate = 600; // rounds per minute
    [SerializeField] private AudioSource shotSound;
    [SerializeField] private ParticleFX muzzleFlash;

    float timeToNextShot;
    public float RotationSpeed = 1;
    public Enemy nearestEnemy;
    float nearestEnemyDistance;
    private RaycastHit raycastHit;


    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<EnemyDrone>())
        {
            var drone = other.GetComponent<EnemyDrone>();
            DronesInRange.Add(drone);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        // remove destroyed drones
        for (int i = DronesInRange.Count - 1; i >= 0; i--)
        {
            if (DronesInRange[i] == null)
            {
                DronesInRange.Remove(DronesInRange[i]);
            }
        }
        // get nearest enemy
        foreach (var drone in DronesInRange)
        {
            if (drone)
            {
                var droneDistance = (turretHead.position - drone.transform.position).sqrMagnitude;
                if (droneDistance < nearestEnemyDistance)
                {
                    nearestEnemyDistance = droneDistance;
                    nearestEnemy = drone;
                }
                else
                {
                    nearestEnemyDistance = Mathf.Infinity;
                }
            }
        }
        // shoot nearest enemy
        if (nearestEnemy)
        {
            SmoothLookAtPosition(nearestEnemy.transform.position, RotationSpeed);
            bool onEnemy = Physics.Raycast(turretHead.position, turretHead.forward, out raycastHit);
            if (onEnemy)
            {
                if (raycastHit.transform.GetComponent<EnemyDrone>())
                {
                    timeToNextShot += Time.deltaTime;
                    if (timeToNextShot > 60 / fireRate)
                    {
                        Shoot();
                        timeToNextShot = 0;
                    }
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<EnemyDrone>())
        {
            var drone = other.GetComponent<EnemyDrone>();
            DronesInRange.Remove(drone);
        }
    }

    void SmoothLookAtPosition(Vector3 position, float speed)
    {
        var targetRotation = Quaternion.LookRotation(position - turretHead.position);
        turretHead.rotation = Quaternion.Slerp(turretHead.rotation, targetRotation, speed * Time.deltaTime);
    }

    EnemyDrone NearestEnemy(EnemyDrone enemy)
    {
        var enemyDistance = Vector3.Distance(turretHead.position, enemy.transform.position);
        return enemy;
    }

    void Shoot()
    {
        // TODO shoot at target!
        projectile.ShootFrom(muzzle, muzzleVelocity);
        if (shotSound)
        {
            shotSound.Play();
        }
        if (muzzleFlash)
        {
            var flash = muzzleFlash.SpawnAt(muzzle.position, 1f);
            flash.gameObject.SetActive(true);
            flash.GetComponent<ParticleSystem>().Play();
        }
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.green;
    //    Gizmos.DrawLine(turretHead.position, raycastHit.transform.position);
    //}
}
