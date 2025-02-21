using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{


    [SerializeField] private Transform _target;
    [SerializeField] private float _smoothTime = 0.25f;

    private Vector3 _offest = new Vector3( 0f, 0f, -10f );
    private Vector3 _volocity = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetPosition = _target.position + _offest;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref _volocity, _smoothTime);
    }
}
