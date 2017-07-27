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
    [SerializeField] private float fireRate = 10; // rounds per second
    [SerializeField] private AudioSource shotSound;
    [SerializeField] private ParticleFX muzzleFlash;

    float timeToNextShot;
    public float RotationSpeed = 1;
    public Enemy nearestEnemy;
    float nearestEnemyDistance;
    private RaycastHit raycastHit;


    private void OnTriggerEnter(Collider other)
    {
        AddEnemyToList(other.transform.root.gameObject);
        RemoveDestroyedEnemiesFrom(DronesInRange);
        GetNearestEnemy(DronesInRange);
    }
    private void OnTriggerExit(Collider other)
    {
        RemoveDestroyedEnemiesFrom(DronesInRange);
        var drone = other.transform.root.GetComponent<EnemyDrone>();
        if (drone)
        {
            DronesInRange.Remove(drone);
        }
        GetNearestEnemy(DronesInRange);
    }
    void AddEnemyToList(GameObject enemy)
    {
        var drone = enemy.GetComponent<EnemyDrone>();
        if (drone)
        {
            DronesInRange.Add(drone);
        }
    }
    void GetNearestEnemy(List<EnemyDrone> enemyList)
    {
        // get nearest enemy
        foreach (var enemy in enemyList)
        {
            if (enemy)
            {
                var droneDistance = (turretHead.position - enemy.transform.position).sqrMagnitude;
                if (droneDistance < nearestEnemyDistance)
                {
                    nearestEnemyDistance = droneDistance;
                    nearestEnemy = enemy;
                }
                else
                {
                    nearestEnemyDistance = Mathf.Infinity;
                }
            }
        }
    }
    void RemoveDestroyedEnemiesFrom(List<EnemyDrone> enemyList)
    {
        // remove destroyed drones
        for (int i = enemyList.Count - 1; i >= 0; i--)
        {
            if (enemyList[i] == null)
            {
                enemyList.Remove(enemyList[i]);
            }
        }
    }
    void SmoothLookAtPosition(Vector3 position, float speed)
    {
        var targetRotation = Quaternion.LookRotation(position - turretHead.position);
        turretHead.rotation = Quaternion.Slerp(turretHead.rotation, targetRotation, speed * Time.deltaTime);
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
            var flash = muzzleFlash.SpawnAtTransform(muzzle, 1f);
            flash.gameObject.SetActive(true);
            flash.GetComponent<ParticleSystem>().Play();
        }
    }

    private void Update()
    {
        timeToNextShot += Time.deltaTime;
        // shoot nearest enemy
        if (nearestEnemy)
        {
            SmoothLookAtPosition(nearestEnemy.transform.position, RotationSpeed);
            bool onEnemy = Physics.Raycast(turretHead.position, turretHead.forward, out raycastHit);
            if (onEnemy)
            {
                if (raycastHit.transform.GetComponent<EnemyDrone>() || raycastHit.transform.root.GetComponent<EnemyDrone>())
                {
                    if (timeToNextShot > 1 / fireRate)
                    {
                        Shoot();
                        timeToNextShot = 0;
                    }
                }
            }
        }
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.green;
    //    Gizmos.DrawLine(turretHead.position, raycastHit.transform.position);
    //}
}
