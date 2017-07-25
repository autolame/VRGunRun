using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDrone : Enemy
{
    public bool IsAimedUpon = false; // set from player gun
    public float EvasionReactionTime = 1f; // seconds
    public float EvasionSpeed = 0.1f;

    public float MinDistanceToTarget = 5f;
    public float MaxDistanceToTarget = 20f;

    public Vector3 directionFromPlayerToEvadeTo;
    float rangeToEvadeTo;

    float timeUntilEvasion;
    bool isEvading = false;


    private void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        IsAimedUpon = true;
    }
    private void Update()
    {
        transform.LookAt(Goal.transform);

        if (IsAimedUpon && !isEvading)
        {
            timeUntilEvasion += Time.deltaTime;
            if (timeUntilEvasion > EvasionReactionTime)
            {
                timeUntilEvasion = 0f;
                isEvading = true;
                IsAimedUpon = false;
                directionFromPlayerToEvadeTo = new Vector3(Random.Range(-1f, 1f), Random.Range(0f, 0.5f), Random.Range(-1f, 1f));
                rangeToEvadeTo = Random.Range(MinDistanceToTarget, MaxDistanceToTarget);
            }
        }

        if (isEvading)
        {
            EvadeToNewPosition(EvasionSpeed);
        }

        if (IsDestroyed)
        {
            foreach (var child in GetComponentsInChildren<Transform>())
            {
                if (!child.gameObject.GetComponent<Rigidbody>())
                {
                    child.gameObject.AddComponent<Rigidbody>();
                }
                Destroy(child.gameObject, 10);
                child.SetParent(null);
            }
            Destroy(gameObject, 10f);
        }
    }

    void EvadeToNewPosition(float speed)
    {
        var playerPosition = Goal.transform.position;
        var positionToMoveTo = playerPosition + directionFromPlayerToEvadeTo * rangeToEvadeTo;

        transform.position = Vector3.MoveTowards(transform.position, positionToMoveTo, speed);

        if (transform.position == positionToMoveTo)
        {
            isEvading = false;
        }
    }
}
