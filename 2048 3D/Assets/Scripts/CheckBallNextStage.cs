using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckBallNextStage : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Ball ball = other.GetComponent<Ball>();
        if (ball != null)
        {
            ball.BallRb.drag = 15f;
            ball.BallRb.drag = 0.5f;
            ball.BallRb.isKinematic = true;
            ball.BallRb.isKinematic = false;

            if (ball.BallRb.velocity.magnitude >= 7f)
                ball.BallRb.velocity = Vector3.ClampMagnitude(ball.BallRb.velocity, 7f);
            if (ball.BallRb.velocity.magnitude < 0.1f)
                ball.BallRb.AddForce(Vector3.down * 7f, ForceMode.Impulse);
        }
    }
}
