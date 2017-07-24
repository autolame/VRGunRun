//======= Copyright (c) Viet Kien Nguyen, All rights reserved. ===============
//
// Purpose: bullet for the Gun
//
//=============================================================================

using UnityEngine;
using System.Collections;


//-------------------------------------------------------------------------
public class GunAmmoBullet : MonoBehaviour
{
    private GameManager gameManager;
    public ParticleFX hitFX;
    float lifeTime = 5f;
    float elapsedTime;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        if (!gameManager)
        {
            Debug.LogWarning(gameObject.name + " CANNOT FIND GAME MANAGER");
        }
    }
    private void Start()
    {
        gameManager.NumberOfShotFired++;
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime > lifeTime)
        {
            gameManager.NumberOfShotMissed++;
            Destroy(gameObject);
        }
    }

    public void ShootFrom(Transform muzzle, float velocity)
    {
        GunAmmoBullet projectile = Instantiate(this, muzzle.position, muzzle.rotation);
        projectile.GetComponent<Rigidbody>().velocity = muzzle.transform.forward * velocity;
        //projectile.gameObject.AddComponent<DestroyObjectAfterSeconds>().TimeSecondToDestroy = 5f;

        elapsedTime = 0;
    }
    private void OnCollisionEnter(Collision collision)
    {
        Vector3 hitPosition = collision.contacts[0].point;

        ParticleFX newFX = hitFX.SpawnAt(hitPosition, 5f);
        newFX.transform.LookAt(transform.position);

        var particles = newFX.GetComponentsInChildren<ParticleSystem>();
        foreach (var particle in particles)
        {
            var main = particle.main;
            main.startSizeMultiplier = .01f;
        }

        if (CountAsHit(collision))
        {
            gameManager.NumberOfShotHit++;
        }
        else
        {
            gameManager.NumberOfShotMissed++;
        }

        Destroy(gameObject);
    }

    bool CountAsHit(Collision collision)
    {
        return collision.collider.GetComponent<TargetDummy>();
    }
}

