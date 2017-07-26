using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [SerializeField] private Transform turretHead;

    public float StartHitPoint = 2500f;
    public List<EnemyDrone> DronesInRange = new List<EnemyDrone>();

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

        // get nearest enemy
        foreach (var drone in DronesInRange)
        {
            if (drone == null)
            {
                DronesInRange.Remove(drone);
            }

            var droneDistance = Vector3.Distance(turretHead.transform.position, drone.transform.position);
            if (droneDistance < nearestEnemyDistance)
            {
                nearestEnemyDistance = droneDistance;
                nearestEnemy = drone;
            }
            else
            {
                nearestEnemyDistance = 20;
            }
        }

        if (nearestEnemy)
        {
            SmoothLookAtPosition(nearestEnemy.transform.position, RotationSpeed);
            bool onEnemy = Physics.Raycast(turretHead.transform.position, transform.forward, out raycastHit);
            if (raycastHit.transform.GetComponent<EnemyDrone>())
            {
                Shoot();
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
        var targetRotation = Quaternion.LookRotation(position - turretHead.transform.position);
        turretHead.transform.rotation = Quaternion.Slerp(turretHead.transform.rotation, targetRotation, speed * Time.deltaTime);
    }

    EnemyDrone NearestEnemy(EnemyDrone enemy)
    {
        var enemyDistance = Vector3.Distance(turretHead.transform.position, enemy.transform.position);
        return enemy;
    }

    void Shoot()
    {
        // TODO shoot at target!
    }
}
