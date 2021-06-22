using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedZone : MonoBehaviour
{
    int checkBallCollider;
    private void OnTriggerStay(Collider other)
    {
        Ball ball = other.GetComponent<Ball>();
        if(ball != null)
        {
            if (!ball.IsMainBall && ball.BallRb.velocity.magnitude < 0.01f && ball.BallRb.velocity.magnitude > -0.01f) //ball.BallRb.velocity == Vector3.zero
            {
                checkBallCollider++;
                if (checkBallCollider >= 7)
                {
                    UIManager.Instance.GameOver();
                    AudioManager.Instance.PlaySoudManager(3);
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Ball ball = other.GetComponent<Ball>();
        if (ball != null)
            checkBallCollider = 0;
    }
}
