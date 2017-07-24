using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{

    public List<PlatformMoveButton> MovementButtons = new List<PlatformMoveButton>();
    public PlatformMoveButton LastActivatedButton;
    public float Speed;

    private void Update()
    {
        Vector3 position = transform.position;
        float moveSpeed = Time.deltaTime * Speed;

        foreach (var button in MovementButtons)
        {
            if (button == LastActivatedButton)
            {
                position = position + button.MovementDirection * moveSpeed;
            }
            else
            {
                button.Activated = false;
            }
        }


        transform.position = position;
    }

}
