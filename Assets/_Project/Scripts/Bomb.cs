using UnityEngine;

/// <summary>
/// Initial speed must be assigned to the missle.
/// </summary>
public class Bomb : MonoBehaviour
{
    // x = 0.5 * a * t^2
    private float gravity = 9.8f;
    private float xSpeed = 5f;

    private void Update()
    {
        CalculateYMovement();
        CalculateXMovement();
    }

    public void AssignSpeed(float speed)
    {
        xSpeed = speed;
    }

    private void CalculateYMovement()
    {
        // h = 0.5 * a * t^2
        var time = Time.deltaTime;
        var position = transform.position;
        position.y -= 0.5f * gravity * gravity * time * time;
        transform.position = position;
    }

    private void CalculateXMovement()
    {
        // x = v * t
        var time = Time.deltaTime;
        var position = transform.position;
        position.x += xSpeed * time;
        transform.position = position;
    }
}
