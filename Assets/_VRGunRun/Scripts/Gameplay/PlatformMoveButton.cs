using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMoveButton : MonoBehaviour
{
    public Platform Platform;
    public bool Activated;
    public Vector3 MovementDirection;
    private void Update()
    {
        GetComponent<Renderer>().material.color = Activated ? Color.green : Color.red;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<GunAmmoBullet>())
        {
            Activated = !Activated;
            if (Activated)
            {
                Platform.LastActivatedButton = this;
            }
            else
            {
                Platform.LastActivatedButton = null;
            }
        }
    }
}
