using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoingToGameOver : MonoBehaviour
{
    [SerializeField] SpriteRenderer redZone;

    float spawnTimer;
    float tileFlashSpeed = 4;
    Color ogringeColor;

    private void Start()
    {
        ogringeColor = redZone.material.color;
    }
    private void OnTriggerStay(Collider other)
    {
        Ball ball = other.GetComponent<Ball>();
        if (!ball.IsMainBall && ball.BallRb.velocity.magnitude < 0.3f)
        {
            redZone.material.color = Color.Lerp(ogringeColor, Color.red, Mathf.PingPong(spawnTimer * tileFlashSpeed, 1f));//
            spawnTimer += Time.deltaTime;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Ball ball = other.GetComponent<Ball>();
        if (!ball.IsMainBall)
        {
            spawnTimer = 0;
            redZone.material.color = ogringeColor;
        }
        
    }
}
