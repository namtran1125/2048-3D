using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSpawner : MonoBehaviour
{
    public static BallSpawner Instance;


    Queue<Ball> ballsQueue = new Queue<Ball>();
    Queue<Ball> collisionQueue = new Queue<Ball>();
    [SerializeField] int ballsQueueCapacity = 20;
    [SerializeField] bool autoQueueGrow = true;

    [SerializeField] GameObject ballPrefab;
    [SerializeField] Color[] ballColors;
    [SerializeField] float[] sizeBalls;

    [HideInInspector] public int maxBallNumber;
    int maxPower = 12;

    Vector3 defaultSpawnPosition;

    [HideInInspector]
    public List<GameObject> Children;

    [SerializeField]
    private Color blackBall;

    int _minValue;
    int currentMinValue;
    int valueRandom;
    private void Awake()
    {
        Instance = this;
        defaultSpawnPosition = transform.position;
        maxBallNumber = (int)Mathf.Pow(2, maxPower);
        InitializeBallsQueue();
        currentMinValue = 0;
    }

    void InitializeBallsQueue()
    {
        for (int i = 0; i < ballsQueueCapacity; i++)
        {
            AddBallToQueue();
            if(i < 10)
                AddBallCollisionToQueue();
        }
    }

    void AddBallToQueue()
    {
        Ball ball = Instantiate(ballPrefab, defaultSpawnPosition, Quaternion.identity, transform).GetComponent<Ball>();

        ball.gameObject.SetActive(false);
        ball.IsMainBall = false;
        ballsQueue.Enqueue(ball);
        Debug.Log("add ball_1 && queue_1 = " + ballsQueue.Count);
    }

    void AddBallCollisionToQueue()
    {
        Ball ball = Instantiate(ballPrefab, defaultSpawnPosition, Quaternion.identity, transform).GetComponent<Ball>();

        ball.gameObject.SetActive(false);
        ball.IsMainBall = false;
        ball.IsQueueCollision = true;
        collisionQueue.Enqueue(ball);
        Debug.Log("add ball_2 && queue_2 = " + collisionQueue.Count);
    }

    public Ball Spawn(int _num, Vector3 _pos)
    {
        if (ballsQueue.Count == 1)
        {
            if (autoQueueGrow)
            {
                Debug.Log("Queue_1 count");
                ballsQueueCapacity++;
                AddBallToQueue();
            }
            else
            {
                Debug.LogError("Err");
                return null;
            }
        }
        Ball ball = ballsQueue.Dequeue();
        ball.transform.position = _pos;
        ball.SetNumber(_num, valueRandom);
        ball.SetColor(GetColor(_num));
        ball.SetSize(GetSize(_num));
        ball.gameObject.SetActive(true);
        return ball;
    }

    public Ball SpawnCollision(int _num, Vector3 _pos)
    {
        if (collisionQueue.Count == 1)
        {
            if (autoQueueGrow)
            {
                Debug.Log("Queue_2 count");
                ballsQueueCapacity++;
                AddBallCollisionToQueue();
            }
            else
            {
                Debug.LogError("Err");
                return null;
            }
        }
        int v = _num + 1;
        Ball ball = collisionQueue.Dequeue();
        ball.ValueRandom = valueRandom;
        ball.transform.position = _pos;
        ball.SetNumber((int)Mathf.Pow(2, v), v);
        ball.SetColor(GetColorBallColl(v));
        ball.SetSize(GetSizeBallColl(v));
        ball.gameObject.SetActive(true);

        return ball;
    }

    public Ball SpawnRandom()
    {
        _minValue = (int)GameManager.Instance.Level_Stage / 2;
        return Spawn(GenarateRandomNumber(_minValue), defaultSpawnPosition);
    }

    public void DesTroyBall(Ball _ball)
    {
        _ball.Light.Stop();
        _ball.BallRb.velocity = Vector3.zero;
        _ball.BallRb.angularVelocity = Vector3.zero;
        _ball.transform.rotation = Quaternion.identity;
        _ball.IsMainBall = false; 
        _ball.parentTxt.transform.parent = _ball.transform;
        _ball.parentTxt.transform.localRotation = Quaternion.identity;
        _ball.gameObject.SetActive(false);

        if (_ball.IsQueueCollision)
            collisionQueue.Enqueue(_ball);
        else
            ballsQueue.Enqueue(_ball);
    }
    public int GenarateRandomNumber(int _minValue)
    {
        int minValue = _minValue;
        int maxValue = minValue + 5;
        if (minValue < 1)
        {
            minValue = 1;
            maxValue = 5;
        }
        valueRandom = Random.Range(minValue, maxValue);
        return (int)Mathf.Pow(2, valueRandom);
    }

    //private Color GetColor(int _num)
    //{
    //    int value = (int)(Mathf.Log(_num) / Mathf.Log(2));
    //    if (value > 12)
    //    {
    //        value = value % 12;
    //    }
    //    return ballColors[value - 1];
    //}

    private Color GetColor(int _num)
    {
        int value = valueRandom;
        if (value > 12)
        {
            value = value % 12;
            if (value == 0)
                value = 12;
        }
        return ballColors[value - 1];
    }

    private Color GetColorBallColl(int _num)
    {
        int value = _num;
        if (value > 12)
        {
            value = value % 12;
            if (value == 0)
                value = 12;
        }
        return ballColors[value - 1];
    }

    private float GetSize(int _num)
    {
        int valueArray = _minValue;
        int a = valueRandom;
        //Debug.Log("a " + a + " min value " + _minValue + " num " + _num);
        if (valueArray < 1)
            valueArray = 1;
        return sizeBalls[a - valueArray];
    }

    private float GetSizeBallColl(int _num)
    {
        int valueArray = _minValue;
        int a = _num;
        if (valueArray < 1)
            valueArray = 1;
        return sizeBalls[a - valueArray];
    }
    public void GetAllBall()
    {
        Children.Clear();
        foreach (Transform child in transform)
        {
            if (child.gameObject.activeInHierarchy && !child.GetComponent<Ball>().IsMainBall)
            {
                Children.Add(child.gameObject);
            }
        }
    }

    public void CheckDestroyBallSmall()
    {
        _minValue = (int)GameManager.Instance.Level_Stage / 2;
        Debug.Log(_minValue + " min value");
        Debug.Log(currentMinValue + " current value");
        if (currentMinValue < _minValue)
        {
            //Destroy balls small
            GetAllBall();
            for (int i = 0; i < Children.Count; i++)
            {
                if (Children[i].GetComponent<Ball>().BallNumber <= (int)Mathf.Pow(2, currentMinValue) || Children[i].GetComponent<Ball>().BallNumber == 0)
                {
                    DesTroyBall(Children[i].GetComponent<Ball>());
                }
            }
            currentMinValue = _minValue;

            //ballsQueue.Clear();
            //collisionQueue.Clear();
            //InitializeBallsQueue();
        }
    }

    public Ball SetBlackBall()
    {
        Ball ball = ballsQueue.Dequeue();
        ball.transform.position = defaultSpawnPosition;
        ball.SetNumber(0, 0);
        ball.SetColor(blackBall);
        ball.SetSize(1.3f);
        ball.gameObject.SetActive(true);
        ball.Light.Play();
        return ball;
    }

    //public Ball SetBallCollision(Ball newBall, int _num, Vector3 _pos)
    //{
    //    newBall.transform.position = _pos;
    //    newBall.SetNumber(_num);
    //    newBall.SetColor(GetColor(_num));
    //    newBall.SetSize(GetSize(_num));

    //    return newBall;
    //}

    #region  Tool DEV
    public Ball SpawnToolBall(int valueBall, Vector2 spawPos)
    {
        _minValue = valueBall;
        valueRandom = valueBall;
        return Spawn(GenarateToolBall(_minValue), new Vector3(spawPos.x, spawPos.y, defaultSpawnPosition.z));
    }
    public int GenarateToolBall(int _minValue)
    {
        return (int)Mathf.Pow(2, _minValue);
    }
    #endregion

    //public void GetBallArray()
    //{
    //    Transform[] allBall = transform.GetComponentsInChildren<Transform>();
    //    for (int i = 0; i < allBall.Length; i++)
    //    {
    //        int a = allBall[i].GetComponent<Ball>().BallNumber;
    //    }  
    //}
}
