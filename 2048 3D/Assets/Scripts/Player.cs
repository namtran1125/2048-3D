using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] float pushForce;
    [SerializeField] float ballMaxPosX;
    [Space]
    [SerializeField] TouchSlider touchSlider;

    Ball mainBall;
    bool isPointerDown;
    bool canMove;
    Vector3 ballPos;

    // Destroy ball onclick
    RaycastHit hit;
    Ray ray;
    public bool checkDestroyBall;


    int vitri;
    // Magnet
    public List<int> equaNum;
    [SerializeField] Transform magnetPos;
    int vitriBallEqua;
    bool hadFound;

    // Delay Explosion
    float timeDelayExplosion = 6f;

    // Tool DEV
    [SerializeField] Camera cam;
    public bool ToolBall;
    public int value;
    public bool blackBall;
    void Start()
    {
        SpawnBall();
        canMove = true;
        touchSlider.OnPointerDownEvent += OnPointerDown;
        touchSlider.OnPointerDragEvent += OnPointerDrag;
        touchSlider.OnPointerUpEvent += OnPointerUp;
    }

    void OnPointerDown()
    {
        if (!checkDestroyBall && !ToolBall)
            isPointerDown = true;
    }
    void OnPointerDrag(float _xMovement)
    {
        if (isPointerDown)
        {
            ballPos = mainBall.transform.position;
            ballPos.x = _xMovement * ballMaxPosX;
        }
    }
    void OnPointerUp()
    {
        if (isPointerDown && canMove && !checkDestroyBall && !ToolBall)
        {
            isPointerDown = false;
            canMove = false;
            AudioManager.Instance.PlaySoudManager(2);
            mainBall.IsMainBall = false;
            mainBall.GetComponent<BallCollision>().GemImpact = true;
            mainBall.SetCollider(true);
            mainBall.BallRb.isKinematic = false;
            mainBall.BallRb.AddForce(Vector3.down * pushForce, ForceMode.Impulse);
            float randomValue = Random.Range(-50f, 50f);
            Vector3 randomDirection = Vector3.one * randomValue;
            mainBall.BallRb.AddTorque(randomDirection);
            GameManager.Instance.CheckDelayExplosion = false;
            Invoke("SpawnBall", 0.7f);
        }
    }

    private void Update()
    {
        CheckDelayExplosion();
        if (isPointerDown && mainBall.IsMainBall)
            mainBall.transform.position = Vector3.Lerp(mainBall.transform.position, ballPos, moveSpeed * Time.deltaTime);

        if (Input.GetMouseButtonDown(0)) {
            if (checkDestroyBall)
            {
                ClickDestroyBall();
            }
            if(ToolBall)
                ToolCheckBall();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            #region old code
            //BallSpawner.Instance.Children.Clear();
            //BallSpawner.Instance.GetAllBall();
            //foreach (var item in BallSpawner.Instance.Children)
            //{
            //    Debug.Log(item.GetComponent<Ball>().BallNumber);
            //    ballsNumber.Add(item.GetComponent<Ball>());
            //}
            //Debug.Log(ballsNumber.Min().BallNumber);
            //Debug.Log(ballsNumber.Max().BallNumber);
            //for (int i = 0; i < 3; i++)
            //{

            //}

            //BallSpawner.Instance.Children.Clear();
            #endregion
            StartCoroutine(DesBallSmall());
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            MagnetBall();
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            GameManager.Instance.NextStage();
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            GameManager.Instance.UpdateCurrentExp(350);
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            
        }


        if (Input.GetKeyDown(KeyCode.I))
        {
            SetWhiteBall();
        }
    }

    void SpawnBall()
    {
        canMove = true;
        int r = Random.Range(0, 100);
        if (r < 5 || blackBall)
        {
            mainBall = BallSpawner.Instance.SetBlackBall();
        }
        else
        {
            mainBall = BallSpawner.Instance.SpawnRandom();
        }
        mainBall.IsMainBall = true;
        mainBall.BallRb.isKinematic = true;
        mainBall.SetCollider(false);
        ballPos = mainBall.transform.position;
    }

    public void SetClickDestroyBall()
    {
        BallSpawner.Instance.GetAllBall();
        if (BallSpawner.Instance.Children.Count > 0)
        {
            checkDestroyBall = true;
        }
    }

    void ClickDestroyBall()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, float.MaxValue))
        {
            if (!hit.collider.gameObject.GetComponent<Ball>().IsMainBall) // hit.collider.gameObject.CompareTag("Ball") && 
            {
                hit.collider.gameObject.GetComponent<Ball>().parentTxt.transform.parent = hit.collider.gameObject.transform;
                Destroy(hit.collider.gameObject);
                checkDestroyBall = false;
            }
            else
                return;
        }
    }

    public IEnumerator DesBallSmall()
    {
        DestroyThreeBallSmall();
        yield return new WaitForSeconds(0.3f);
        DestroyThreeBallSmall();
        yield return new WaitForSeconds(0.3f);
        DestroyThreeBallSmall();
    }

    void DestroyThreeBallSmall()
    {
        BallSpawner.Instance.GetAllBall();
        if(BallSpawner.Instance.Children.Count > 0)
        {
            int minNumberBall = BallSpawner.Instance.Children[0].GetComponent<Ball>().BallNumber;
            for (int j = 0; j < BallSpawner.Instance.Children.Count; j++)
            {
                if (minNumberBall >= BallSpawner.Instance.Children[j].GetComponent<Ball>().BallNumber)
                {
                    minNumberBall = BallSpawner.Instance.Children[j].GetComponent<Ball>().BallNumber;
                    vitri = j;
                }
            }
            Debug.Log("Min " + minNumberBall);
            //Destroy(BallSpawner.Instance.Children[vitri]);
            BallSpawner.Instance.DesTroyBall(BallSpawner.Instance.Children[vitri].GetComponent<Ball>());
            BallSpawner.Instance.Children.RemoveAt(vitri);
        }
        
    }

    public void MagnetBall()
    {
        equaNum.Clear();
        BallSpawner.Instance.GetAllBall();
        // Tim so bang nhau
        for (int i = 0; i < BallSpawner.Instance.Children.Count; i++)
        {
            for (int j = i + 1; j < BallSpawner.Instance.Children.Count; j++)
            {
                if (BallSpawner.Instance.Children[i].GetComponent<Ball>().BallNumber == BallSpawner.Instance.Children[j].GetComponent<Ball>().BallNumber)
                {
                    //equaNum.Add(i);
                    //equaNum.Add(j);
                    hadFound = true;
                    BallSpawner.Instance.Children[i].GetComponent<Ball>().BallRb.MovePosition(magnetPos.transform.position);
                    BallSpawner.Instance.Children[j].GetComponent<Ball>().BallRb.MovePosition(magnetPos.transform.position);
                    break;
                }
            }
            if (hadFound)
            {
                hadFound = false;
                break;
            }
        }
        #region Tim so lon nhat
        // Tim so lon nhat
        //if (equaNum.Count > 0)
        //{
        //    int maxNum = BallSpawner.Instance.Children[equaNum[0]].GetComponent<Ball>().BallNumber;
        //    for (int k = 0; k < equaNum.Count; k++)
        //    {
        //        //Debug.Log(BallSpawner.Instance.Children[equaNum[k]].GetComponent<Ball>().BallNumber);
        //        if (maxNum <= BallSpawner.Instance.Children[equaNum[k]].GetComponent<Ball>().BallNumber)
        //        {
        //            maxNum = BallSpawner.Instance.Children[equaNum[k]].GetComponent<Ball>().BallNumber;
        //            vitriBallEqua = k;
        //        }
        //    }
        //    //Debug.Log(BallSpawner.Instance.Children[equaNum[vitriBallEqua]].GetComponent<Ball>().BallNumber + "max num");
        //    //Debug.Log(BallSpawner.Instance.Children[equaNum[vitriBallEqua - 1]].GetComponent<Ball>().BallNumber);

        //    //BallSpawner.Instance.Children[equaNum[vitriBallEqua]].GetComponent<Ball>().BallRb.AddForce(new Vector3(0, 1f, 0) * pushForce, ForceMode.Impulse);
        //    //BallSpawner.Instance.Children[equaNum[vitriBallEqua - 1]].GetComponent<Ball>().BallRb.AddForce(new Vector3(0, 1f, 0) * pushForce, ForceMode.Impulse);

        //    BallSpawner.Instance.Children[equaNum[vitriBallEqua]].GetComponent<Ball>().BallRb.MovePosition(magnetPos.transform.position);
        //    BallSpawner.Instance.Children[equaNum[vitriBallEqua - 1]].GetComponent<Ball>().BallRb.MovePosition(magnetPos.transform.position);
        //}
        //else
        //{
        //    Debug.Log("ko co 2 so bang nhau");
        //}
        #endregion
    }

    public void SetWhiteBall()
    {
        mainBall.WhiteBall = true;
        mainBall.SetColor(Color.white);
    }

    public void SetMainBallNextStage(bool _ball)
    {
        mainBall.gameObject.SetActive(_ball);
        if (!_ball)
        {
            mainBall.parentTxt.transform.parent = mainBall.transform;
            mainBall.parentTxt.transform.localRotation = Quaternion.identity;
        }
        else
        {
            mainBall.parentTxt.transform.parent = GameManager.Instance.ParentTxt.transform;
        }
    }

    void CheckDelayExplosion()
    {
        if (GameManager.Instance.CheckDelayExplosion)
        {
            //timeDelayExplosion -= Time.fixedDeltaTime;
            //if (timeDelayExplosion <= 0.1f)
            //{
            //    timeDelayExplosion = 0.6f;
            //    if (GameManager.Instance.CheckDelayExp)
            //    {
            //        GameManager.soudPos = 0;
            //        GameManager.Instance.CheckDelayExp = false;
            //    }

            //    Debug.Log("aaaa");
            //}
        }
        else
        {// ko delay
            timeDelayExplosion -= Time.fixedDeltaTime;
            if (timeDelayExplosion <= 0.1f)
            {
                timeDelayExplosion = 0.5f;
                if (!GameManager.Instance.CheckDelayExplosion) // sau 0.5s tiep tuc ko delay || ko combo thi set sound = 0
                {
                    GameManager.soudPos = 0;
                }
            }
        }
    }

    private void OnDestroy()
    {
        touchSlider.OnPointerDownEvent -= OnPointerDown;
        touchSlider.OnPointerDragEvent -= OnPointerDrag;
        touchSlider.OnPointerUpEvent -= OnPointerUp;
    }

    void ToolCheckBall()
    {
        Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        BallSpawner.Instance.SpawnToolBall(value, mousePos);


        //ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //if (Physics.Raycast(ray, out hit, float.MaxValue))
        //{
        //    BallCollision b = hit.collider.gameObject.GetComponent<BallCollision>();
        //    if (b)
        //    {
        //        //Debug.Log(b.isDelay + " CHeck Delay ball");
        //    }
        //    else
        //        return;
        //}
    }

}

#region Old code Magnet
/*
 https://www.youtube.com/watch?v=6o_NBh3kdFw
 */
//void TestMaget()
//{
//    BallSpawner.Instance.GetAllBall();
//    int maxNumberBall = BallSpawner.Instance.Children[0].GetComponent<Ball>().BallNumber;
//    for (int i = 0; i < BallSpawner.Instance.Children.Count; i++)
//    {
//        if (maxNumberBall <= BallSpawner.Instance.Children[i].GetComponent<Ball>().BallNumber)
//        {
//            maxNumberBall = BallSpawner.Instance.Children[i].GetComponent<Ball>().BallNumber;
//            vitri = i;
//        }
//    }
//    Debug.Log("Max " + maxNumberBall);
//}
#endregion
