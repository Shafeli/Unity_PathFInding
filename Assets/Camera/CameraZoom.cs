using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
   
    [SerializeField] private float _zoomMultiplier = 4f;
    [SerializeField] private float _minZoom = 2f;
    [SerializeField] private float _maxZoom = 8f;
    
    [SerializeField] private float _smoothTime = 0.25f;

    private float _zoom;
    private float _zoomVelocity = 0f;
    private Camera _camera = null;

    // Start is called before the first frame update
    void Start()
    {
        _camera = GetComponent<Camera>();
        _zoom = _camera.orthographicSize;
    }

    // Update is called once per frame
    void Update()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        _zoom -= scroll * _zoomMultiplier; // #saveTheWrists

        _zoom = Mathf.Clamp(_zoom, _minZoom, _maxZoom);
        _camera.orthographicSize = Mathf.SmoothDamp(_camera.orthographicSize, _zoom, ref _zoomVelocity, _smoothTime);
    }
}
