using UnityEngine;

public class CameraMover : MonoBehaviour
{
    private float speed = 0.1f;
    private float left_boundary = -1.5f;
    private float right_boundary = 8.5f;
    private float front_boundary = -2f;
    private float back_boundary = 14.5f;
    void Update()
    {
        Vector3 position = transform.position;
        if (Input.GetMouseButton(0) && Input.GetKey(KeyCode.LeftControl) && !Input.GetKey(KeyCode.LeftShift))
        {
            position.y = position.y + Input.GetAxis("Mouse Y") * 2 * speed;
        }
        if (Input.GetMouseButton(0) && !Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftShift))
        {
            position.x = position.x + Input.GetAxis("Mouse X") * 2 * speed;
            position.z = position.z + Input.GetAxis("Mouse Y") * 2 * speed;
        }
        
        position.x = position.x > right_boundary ? right_boundary : position.x;
        position.x = position.x < left_boundary? left_boundary : position.x;
        position.z = position.z < front_boundary ? front_boundary : position.z;
        position.z = position.z > back_boundary ? back_boundary : position.z;
        transform.position = position;
    }
}
