using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class WhiteboardCameraController : MonoBehaviour
{
    public Material whiteboardMaterial;

    // Settings
    private const int Framerate = 30;

    // Internal
    private Camera _camera;
    private Texture2D _colorTex;
    private double _lastRenderTime;
    private float _oneOverFramerate;

    
    
    private void Start()
    {
        _camera = GetComponent<Camera>();
        //_camera.targetTexture = _renderTexture;
        
        _lastRenderTime = 0;
        _oneOverFramerate = 1 / (float) Framerate;
    }

    private void Update()
    {
        double time = Time.timeAsDouble;
        if (time > _lastRenderTime + _oneOverFramerate)
        {
            _lastRenderTime = time;
            _camera.Render();
        }
    }


    private void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        
    }
}
