using UnityEngine;

public class CameraMover : MonoBehaviour
{
    private float speed = 0.1f;
    private float left_boundary = -1.5f;
    private float right_boundary = 8.5f;
    private float front_boundary = -1.5f;
    private float back_boundary = 14.5f;
    void Update()
    {
        Vector3 position = transform.position;
        if (Input.GetAxis("Horizontal") < 0 && position.x > left_boundary) 
        {
            position.x -= speed;
        }
        if (Input.GetAxis("Horizontal") > 0 && position.x < right_boundary)
        {
            position.x += speed;
        }

        if (Input.GetAxis("Vertical") > 0 && position.z > front_boundary)
        {
            position.z += speed;
        }
        if (Input.GetAxis("Vertical") < 0 && position.z < back_boundary)
        {
            position.z -= speed;
        }
        transform.position = position;
        
        print(Input.GetAxis("Horizontal") + " " + Input.GetAxis("Vertical") + " " + position.x + " " + position.z);
    }
}
