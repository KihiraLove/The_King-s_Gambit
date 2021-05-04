using System;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float lookSpeedH = 1.6f;

    [SerializeField] private float lookSpeedV = 1.6f;

    [SerializeField] private float zoomSpeed = 6f;

    [SerializeField] private float dragSpeed = 32f;

    private float _yaw;
    private float _pitch;

    private void Start()
    {
        // Initialize the correct initial rotation
        UpdateRotation();
    }

    private void Update()
    {
        //Look around with Right Mouse
        if (Input.GetMouseButton(1))
        {
            _yaw += lookSpeedH * Input.GetAxis("Mouse X");
            _pitch -= lookSpeedV * Input.GetAxis("Mouse Y");

            transform.eulerAngles = new Vector3(_pitch, _yaw, 0f);
        }

        //drag camera around with Middle Mouse
        if (Input.GetMouseButton(2))
        {
            transform.Translate(-Input.GetAxisRaw("Mouse X") * Time.deltaTime * dragSpeed,
                -Input.GetAxisRaw("Mouse Y") * Time.deltaTime * dragSpeed, 0);
        }

        //Zoom in and out with Mouse Wheel
        transform.Translate(0, 0, Input.GetAxis("Mouse ScrollWheel") * zoomSpeed, Space.Self);
    }

    public void Recenter()
    {
        transform.position = new Vector3(3.5f, 8f, -9f);
        transform.rotation = Quaternion.identity;

        UpdateRotation();
    }

    private void UpdateRotation()
    {
        var eulerAngles = transform.eulerAngles;
        _yaw = eulerAngles.y;
        _pitch = eulerAngles.x;
    }
}