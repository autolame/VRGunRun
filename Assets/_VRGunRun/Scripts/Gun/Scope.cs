using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scope : MonoBehaviour
{
    public Camera RTCamera;
    public Renderer RenderTarget;

    public RenderTexture RenderTexture;

    public float MinZoomFOV = 10;
    public float MaxZoomFOV = 1;


    private void LateUpdate()
    {
        RenderTexture = RTCamera.targetTexture;
    }
}
