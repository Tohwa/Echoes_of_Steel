using UnityEngine;

public class Dash : MonoBehaviour
{
    public float dashSpeed = 20f;
    public float dashTime = 0.2f;
    public float dashCooldown = 3f;
    private bool isDashing = false;
    private float dashTimeLeft;
    private float lastDashTime = -10f;

    public NodeState PerformDash()
    {
        if (Time.time > lastDashTime + dashCooldown)
        {
            StartDash();
            return NodeState.SUCCESS;
        }
        return NodeState.FAILURE;
    }

    private void StartDash()
    {
        isDashing = true;
        dashTimeLeft = dashTime;
        lastDashTime = Time.time;
    }

    void Update()
    {
        if (isDashing)
        {
            if (dashTimeLeft > 0)
            {
                Vector3 direction = transform.forward; // Assuming the dash direction is forward
                transform.position += direction * dashSpeed * Time.deltaTime;

                RotateInDirection(direction);

                dashTimeLeft -= Time.deltaTime;
            }
            else
            {
                isDashing = false;
            }
        }
    }

    private void RotateInDirection(Vector3 direction)
    {
        if (direction.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10f * Time.deltaTime);
        }
    }
}