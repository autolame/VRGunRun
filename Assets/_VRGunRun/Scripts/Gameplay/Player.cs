using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    const float defaultHitPoints = 100f;
    const float defaultArmor = 0f;

    [SerializeField] private float hitPoints = 100f;
    [SerializeField] private float armor = 0f;

    [SerializeField] private float respawnTime = 5f;
    [SerializeField] private float timeSpentDead = 0f;

    private void OnCollisionEnter(Collision collision)
    {

    }
}
