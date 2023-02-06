using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class FixController : MonoBehaviour
{
    public Transform camera;
    
    // Settings
    private const float DrawingWidth = 0.003f;
    private const int LineSteps = 4;
    
    // Private
    private LineRenderer _lineRenderer;
    private Vector3 _lastTipPosition;
    private Queue<Vector3> _positions;
    
    
    void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();

        _lineRenderer.positionCount = LineSteps;
        _lineRenderer.startWidth = DrawingWidth;
        _lineRenderer.endWidth = DrawingWidth;
        _lineRenderer.numCapVertices = 3;
        _lineRenderer.numCornerVertices = 3;

        _lineRenderer.enabled = true;

        _positions = new Queue<Vector3>();
    }

    
    void Update()
    {
        transform.rotation = camera.rotation;
        _positions.Enqueue(transform.position);
        if (_positions.Count > LineSteps)
            _positions.Dequeue();
        _lineRenderer.SetPositions(_positions.ToArray());
    }
}
