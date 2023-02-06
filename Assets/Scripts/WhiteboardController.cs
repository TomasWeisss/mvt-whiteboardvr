using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WhiteboardController : MonoBehaviour
{
    public Camera renderCamera;
    public Vector2 boardSize;
    public int materialIndex = 0;
    public Transform tip;
    public Transform bottomLeft;
    public Transform topRight;

    // Settings
    private const int RenderLayer = 6;
    private const int RenderFps = 20;
    private const int RenderResolution = 2160;
    private const int BlockSize = 4;

    // Private
    private Texture2D _colorTexture;
    private Renderer _renderer;
    private double _lastRenderTime;
    private float _oneOverFramerate;
    private Color[] _colors;
    private Vector2? _lastPos;
    
    
    
    void Start()
    {
        _renderer = GetComponent<Renderer>();
        
        int texWidth = RenderResolution * (int) Mathf.Floor(boardSize.x / boardSize.y);

        _lastRenderTime = 0;
        _oneOverFramerate = 1 / (float) RenderFps;
        
        _colorTexture = new Texture2D(texWidth, RenderResolution, TextureFormat.ARGB32, false)
        {
            filterMode = FilterMode.Bilinear
        };
        _renderer.sharedMaterials[materialIndex].SetTexture("_ColorTex", _colorTexture);

        _colors = new Color[BlockSize * BlockSize];
        for (int y = 0; y < BlockSize; y++)
        for (int x = 0; x < BlockSize; x++)
        {
            var xF = (float)x / BlockSize - .5f;
            var yF = (float)y / BlockSize - .5f;
            
            //float weight = Mathf.Exp(-4 * Mathf.Pow(xF, 2));

            _colors[x + y * BlockSize] = Color.blue;
        }
    }

    
    private void Update()
    {
        Render();
    }


    private void Render()
    {
        // Get pos
        var diff = tip.transform.position - bottomLeft.transform.position;
        var pos = new Vector2(diff.x / (topRight.position.x - bottomLeft.position.x),
            diff.y / (topRight.position.y - bottomLeft.position.y));

        // Skip if out of bounds
        if (pos.x is > 1 or < 0 || pos.y is > 1 or < 0 || diff.z is < -0.02f or > 0.1f)
        {
            _lastPos = null;
            return;
        }
        
        // Render pixels
        for (float t = 0; t <= 1; t += 0.05f)
        {
            if (_lastPos == null)
            {
                _lastPos = pos;
                t = 1;
            }

            var lerpPos = Vector2.Lerp((Vector2)_lastPos, pos, t);
            int x = (int)((1-lerpPos.x) * _colorTexture.width);
            int y = (int)(lerpPos.y * _colorTexture.height);
            _colorTexture.SetPixels(x-BlockSize/2, y - BlockSize/2, BlockSize, BlockSize, _colors);
        }
        
        _colorTexture.Apply();

        _lastPos = pos;
    }
}
