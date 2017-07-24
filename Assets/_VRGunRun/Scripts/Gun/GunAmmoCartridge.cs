//======= Copyright (c) Viet Kien Nguyen, All rights reserved. ===============
//
// Purpose: empty cartridge
//
//=============================================================================

using UnityEngine;
using System.Collections;
using Valve.VR.InteractionSystem;

//-------------------------------------------------------------------------------------------------
public class GunAmmoCartridge : MonoBehaviour
{

    private bool timedDestroy = false;
    private float lifeTime = 10f; // TODO find cleanup time : 5min!

    public float LifeTime
    {
        get { return lifeTime; }
        set { lifeTime = value; }
    }

    public void EjectFrom(Transform ejectionPort, float velocity)
    {
        GunAmmoCartridge projectile = Instantiate(this, ejectionPort.position, ejectionPort.rotation);
        projectile.GetComponent<Rigidbody>().velocity = ejectionPort.transform.right * velocity;
        projectile.GetComponent<Rigidbody>().angularVelocity = new Vector3(Random.Range(-100, 100), Random.Range(-1000, 1000), Random.Range(-100, 100));
    }

    private void OnCollisionEnter(Collision collision)
    {
        timedDestroy = true;
    }

    private void FixedUpdate()
    {
        if (timedDestroy)
        {
            LifeTime -= Time.deltaTime;
            if (LifeTime < 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
