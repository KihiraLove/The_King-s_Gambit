﻿using UnityEngine;

[AddComponentMenu("Camera Control")]
public class CameraController : MonoBehaviour
{
    public Transform target;
    public float distance = 10.0f;
    public float xSpeed = 100.0f;
    public float ySpeed = 100.0f;

    public float yMinLimit = -20f;
    public float yMaxLimit = 80f;

    public float distanceMin = 1f;
    public float distanceMax = 20f;

    public Texture2D _cursorTexture;
    private readonly CursorMode _cursorMode = CursorMode.Auto;

    private float _x;
    private float _y;

    private void Start()
    {
        var angles = transform.eulerAngles;
        _x = angles.y;
        _y = angles.x;
    }

    private void Update()
    {
        if (Input.GetMouseButton(0) && !Input.GetKey(KeyCode.LeftControl) && !Input.GetKey(KeyCode.LeftShift))
        {
            _x += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
            _y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;
            _y = ClampAngle(_y, yMinLimit, yMaxLimit);
            Cursor.SetCursor(_cursorTexture, Vector2.zero, _cursorMode);
        }
        else
        {
            Cursor.SetCursor(null, Vector2.zero, _cursorMode);
        }

        var rotation = Quaternion.Euler(_y, _x, 0);
        distance = Mathf.Clamp(distance - Input.GetAxis("Mouse ScrollWheel") * 5, distanceMin, distanceMax);

        RaycastHit hit;

        var _layerMask = 1 << 9;
        if (Physics.Linecast(target.position, transform.position, out hit, _layerMask)) distance -= hit.distance;

        var negDistance = new Vector3(0.0f, 0.0f, -distance);
        var position = rotation * negDistance + target.position;

        transform.rotation = rotation;
        transform.position = position;
    }

    private static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360F)
            angle += 360F;
        if (angle > 360F)
            angle -= 360F;
        return Mathf.Clamp(angle, min, max);
    }
}