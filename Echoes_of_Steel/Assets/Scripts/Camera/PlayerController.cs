using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private Rigidbody rb;
    [SerializeField]
    private float moveSpeed;

    private bool forward;
    private bool left;
    private bool right;
    private bool backward;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if(forward)
        {
            rb.velocity += Vector3.forward * moveSpeed * Time.deltaTime;
        }

        if(left)
        {
            rb.velocity += Vector3.left * moveSpeed * Time.deltaTime;
        }

        if(right)
        {
            rb.velocity += Vector3.right * moveSpeed * Time.deltaTime;
        }

        if(backward)
        {
            rb.velocity += Vector3.back * moveSpeed * Time.deltaTime;
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
