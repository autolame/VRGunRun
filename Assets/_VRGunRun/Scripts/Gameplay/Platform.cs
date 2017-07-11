using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{

    public PlatformMoveButton Forward, Backward, Right, Left;
    public float Speed;

    private void Update()
    {
        Vector3 position = transform.position;
        float moveSpeed = Time.deltaTime * Speed;

        position.z = Forward.Activated ? position.z + moveSpeed : position.z;
        position.z = Backward.Activated ? position.z - moveSpeed : position.z;
        position.x = Right.Activated ? position.x + moveSpeed : position.x;
        position.x = Left.Activated ? position.x - moveSpeed : position.x;

        transform.position = position;
    }

}
