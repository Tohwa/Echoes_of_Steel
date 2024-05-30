using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody rb;
    [SerializeField]
    private float moveSpeed;
    private float maxSpeed = 10f;

    private bool forward;
    private bool left;
    private bool right;
    private bool backward;


    private float rotationSpeed = 10f;

    private CameraController mainCam;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        mainCam = FindObjectOfType<CameraController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (forward)
        {
            rb.velocity += Vector3.forward * moveSpeed * Time.deltaTime;
        }

        if (left)
        {
            if (mainCam.RMBstate)
            {
                rb.velocity += Vector3.left * moveSpeed * Time.deltaTime;
            }
            else
            {
                transform.Rotate(Vector3.up, -rotationSpeed * Time.deltaTime);
            }
        }

        if (right)
        {
            if (mainCam.RMBstate)
            {
                rb.velocity += Vector3.right * moveSpeed * Time.deltaTime;
            }
            else
            {
                transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
            }
        }

        if (backward)
        {
            rb.velocity += Vector3.back * moveSpeed * Time.deltaTime;
        }

        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }


        UserInput();
    }

    void UserInput()
    {
        forward = Input.GetKey(KeyCode.W);
        right = Input.GetKey(KeyCode.D);
        left = Input.GetKey(KeyCode.A);
        backward = Input.GetKey(KeyCode.S);
    }
}
