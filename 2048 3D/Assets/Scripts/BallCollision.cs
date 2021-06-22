using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallCollision : MonoBehaviour
{
    Ball ball;
    float pushForce = 2.5f;
    List<Ball> ballOther = new List<Ball>();
    [HideInInspector]
    public bool GemImpact;
    int valueRandomNewBall;
    Vector3 contactPoint;
    private void Awake()
    {
        ball = GetComponent<Ball>();
    }
    private void OnCollisionEnter(Collision collision) // check 2 ball big duoc tao cung luc 
    {
        //valueRandomNewBall = ValueRandom + 1;
        Ball otherBall = collision.gameObject.GetComponent<Ball>();

        if (GemImpact && collision != null)
        {
            GemImpact = false;
            //if(otherBall)

            AudioManager.Instance.GemImpactSound();
        }

        if (otherBall != null) //  && ball.BallID > otherBall.BallID
        {
            if (ball.WhiteBall)
            {
                ball.SetNumber(otherBall.BallNumber, otherBall.ValueRandom);
                ball.WhiteBall = false;
                Debug.Log("ball.BallNumber " + ball.BallNumber);
            }

            if (ball.BallNumber == otherBall.BallNumber)
            {
                contactPoint = collision.contacts[0].point;
                if (ball.BallNumber == 0)
                {
                    //ExplosionAndSound(); 
                    GameManager.Instance.explosion_BlackBall.transform.position = new Vector3(contactPoint.x, contactPoint.y, -1f);
                    GameManager.Instance.explosion_BlackBall.Play();
                    BallSpawner.Instance.DesTroyBall(ball);
                    BallSpawner.Instance.DesTroyBall(otherBall);
                    Debug.Log("black ball");
                }
                else
                {
                    Collider c = collision.GetContact(0).otherCollider;
                    Ball ob = c.gameObject.GetComponent<Ball>();
                    if (!GameManager.Instance.CheckDelayExplosion)
                    {
                        StartCoroutine(DelayExplosion(contactPoint, ob, 0));
                    }
                    else
                    {
                        StartCoroutine(DelayExplosion(contactPoint, ob, 0.18f));
                    }
                }
            }
            else
            {
                //Debug.Log("not equa number");
                return;
            }

        }
    }

    IEnumerator DelayExplosion(Vector3 contactPoint, Ball otherBall, float time)
    {
        GameManager.Instance.CheckDelayExplosion = true;
        CameraShake.instance.shouldShake = true;

        //Debug.Log(valueRandomNewBall);
        yield return new WaitForSeconds(time);
        //Ball newBall = BallSpawner.Instance.SpawnCollision(ball.BallNumber * 2, contactPoint);
        Ball newBall = BallSpawner.Instance.SpawnCollision(ball.ValueRandom, contactPoint);
        newBall.BallRb.isKinematic = false;
        newBall.BallRb.AddForce(new Vector3(0, 1f, 0) * pushForce, ForceMode.Impulse);
        GameManager.Instance.UpdateCurrentExp(newBall.BallNumber);
        //ball.parentTxt.transform.parent = ball.transform;
        //ball.parentTxt.transform.localRotation = Quaternion.identity;
        //otherBall.parentTxt.transform.parent = otherBall.transform;
        //otherBall.parentTxt.transform.localRotation = Quaternion.identity;

        ExplosionAndSound();

        BallSpawner.Instance.DesTroyBall(ball);
        BallSpawner.Instance.DesTroyBall(otherBall);

        Debug.Log("Destroy ball");
        CameraShake.instance.shouldShake = false;

        #region Next Stage
        if (newBall.BallNumber >= GameManager.Instance.BallLevel)
        {
            Debug.Log(newBall.BallNumber + "WIN" + GameManager.Instance.Level_Stage);
            //Destroy(newBall.gameObject);
            GameManager.Instance.NextStage();
        }
        #endregion
    }

    void ExplosionAndSound()
    {
        #region Explosion & Sound
        GameManager.Instance.explosion.transform.position = new Vector3(contactPoint.x, contactPoint.y, -1f);
        GameManager.Instance.explosion.Play();

        AudioManager.Instance.ComboSound(GameManager.soudPos);
        if (GameManager.soudPos < AudioManager.Instance.combos.Length - 1)
            GameManager.soudPos++;

        //Collider[] surroundedBalls = Physics.OverlapSphere(contactPoint, 1.2f);
        //float explosionForce = 400f;
        //float explosionRadius = 1.5f;
        //foreach (Collider item in surroundedBalls)
        //{
        //    if (item.attachedRigidbody != null)
        //        item.attachedRigidbody.AddExplosionForce(explosionForce, contactPoint, explosionRadius);
        //}
        #endregion
    }
}
