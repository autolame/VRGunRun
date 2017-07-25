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

    private void Awake()
    {
        RenderTexture = new RenderTexture(2048, 2048, 0);
        RenderTexture.Create();
    }

    private void LateUpdate()
    {
        RTCamera.targetTexture = RenderTexture;
        RenderTarget.material.SetTexture("_MainTex", RenderTexture);
        RenderTarget.material.SetTexture("_EmissionMap", RenderTexture);
    }
}
