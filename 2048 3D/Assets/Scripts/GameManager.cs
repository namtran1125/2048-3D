using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [Header("<<-- Data Game -->>")]
    private int level_Stage;
    public int Level_Stage { get { return level_Stage; } }
    private int ballLevel;
    public int BallLevel { get { return ballLevel; } }
    private int level;
    public int Level { get { return level; } }
    private int targetExp;
    public int TargetExp { get { return targetExp; } }
    public int CurrentExp;
    public float GravityBall;

    // Stage Win
    [Header("<<-- Stage Obj -->>")]
    [SerializeField]
    private GameObject stage_1;
    [SerializeField]
    private GameObject stage_2;
    [SerializeField]
    private BoxCollider[] bottomCollider; // enble khi qua stage
    [SerializeField]
    private GameObject[] elasticityCollider; // khi qua stage kéo 2 collider right + left đẩy ball xuống stage tiếp theo 0.8f - 0.35f
    [SerializeField]
    private float speedStage;
    [SerializeField]
    private float speedElasticity;
    bool checkStage;
    float value_PosY_1; // lưu giá trị pos Y của stage để lặp lại position
    float value_PosY_2;
    float value_Elasticity_Collider = 0.8f; // lưu giá trị pos X của Elasticity_Collider để lặp lại position 
    float value_Limit_Elasticity = 0.33f;
    [HideInInspector]
    public bool CheckBallCollision;

    [Header("<<-- Number Text -->>")]
    public TMP_Text[] TextNumberWin;

    [SerializeField] Player player;
    public GameObject ParentTxt;

    [Header("<<-- Effect -->>")]
    public ParticleSystem explosion;
    public ParticleSystem explosion_BlackBall;

    public bool CheckDelayExplosion;

    public static int soudPos;


    void Awake()
    {
        Instance = this;

        // Stage
        level_Stage = 1;
        ballLevel = 256;

        // Progress
        level = 1;
        targetExp = 1200;
        CurrentExp = 0;
        UpdateTextNumber();

    }

    public void NextStage()
    {
        level_Stage++;
        ballLevel *= 2;
        Debug.Log("n");
        BallSpawner.Instance.CheckDestroyBallSmall();
        player.SetMainBallNextStage(false);
        if (!checkStage)
        {
            DoorOpen(0);
        }
        else
        {
            DoorOpen(1);
        }
        StartCoroutine(MovingStage());
    }

    IEnumerator MovingStage()
    {
        float i = 0;
        value_PosY_1 = stage_1.transform.localPosition.y;
        value_PosY_2 = stage_2.transform.localPosition.y;
        //stage.transform.position = new Vector3(0, stage.transform.position.y - 18.85f, 0);
        while (i < 10)
        {
            stage_1.transform.Translate(Vector3.up * speedStage * Time.deltaTime);
            stage_2.transform.Translate(Vector3.up * speedStage * Time.deltaTime);
            if (stage_1.transform.localPosition.y >= value_PosY_1 + 18.85f) // * Level
                i = 10f;
            i += 0.02f;
            yield return null;
        }
        UpdateTextNumber();
        if (!checkStage)
        {
            yield return StageMove(stage_1, value_PosY_2, 0);
        }
        else
        {
            yield return StageMove(stage_2, value_PosY_1, 1);
        }
    }

    IEnumerator StageMove(GameObject _stage, float _value, int _index)
    {
        float k = 0;
        while (k < 10)
        {
            if (!checkStage)
            {
                elasticityCollider[0].transform.Translate(Vector3.left * speedElasticity * Time.deltaTime);
                elasticityCollider[1].transform.Translate(Vector3.right * speedElasticity * Time.deltaTime);
            }
            else
            {
                elasticityCollider[2].transform.Translate(Vector3.left * speedElasticity * Time.deltaTime);
                elasticityCollider[3].transform.Translate(Vector3.right * speedElasticity * Time.deltaTime);
            }

            if (elasticityCollider[0].transform.localPosition.x <= value_Limit_Elasticity || elasticityCollider[2].transform.localPosition.x <= value_Limit_Elasticity)
                k = 10f;

            k += 0.1f;
            yield return null;
        }

        //float i = 0;
        yield return new WaitForSeconds(3.5f);
        //while (i < 10)
        //{
        //    //_stage.transform.localPosition += (Vector3.up * Time.deltaTime);
        //    _stage.transform.Translate(Vector3.up * speedStage * Time.deltaTime);
        //    if (_stage.transform.localPosition.y >= 22f)
        //        i = 10f;
        //    i += 0.2f;
        //    yield return null;
        //}
        _stage.transform.localPosition = new Vector3(0, _value, 0); // reset posotion stage
        checkStage = !checkStage;
        //LockRight[_index].transform.localRotation = Quaternion.Euler(Vector3.zero);
        //LockLeft[_index].transform.localRotation = Quaternion.Euler(Vector3.zero);
        bottomCollider[_index].enabled = true;

        if (checkStage)
        {
            elasticityCollider[0].transform.localPosition = new Vector3(value_Elasticity_Collider, 0, 0);
            elasticityCollider[1].transform.localPosition = new Vector3(-value_Elasticity_Collider, 0, 0);
        }
        else
        {
            elasticityCollider[2].transform.localPosition = new Vector3(value_Elasticity_Collider, 0, 0);
            elasticityCollider[3].transform.localPosition = new Vector3(-value_Elasticity_Collider, 0, 0);
        }

        player.SetMainBallNextStage(true);
        #region Old Code
        //while (i < 10)
        //{
        //    stage_1.transform.Translate(Vector3.up * SpeedStage * Time.deltaTime);
        //    i += 0.2f;
        //}
        //stage_1.transform.localPosition = new Vector3(0, value_2, 0);
        #endregion
    }

    void DoorOpen(int _index)
    {
        bottomCollider[_index].enabled = false;
        //LockRight[_index].transform.localRotation = Quaternion.Euler(0, 0, -60);
        //LockLeft[_index].transform.localRotation = Quaternion.Euler(0, 0, 60);
    }

    public void UpdateTextNumber()
    {
        for (int i = 0; i < TextNumberWin.Length; i++)
        {
            TextNumberWin[i].text = ballLevel.ToString();
        }
    }

    public void UpdateLevelProgress()
    {
        if(CurrentExp >= targetExp)
        {
            level++;
            targetExp *= (int)Random.Range(1.8f, 2.2f);
            CurrentExp = 0;

            AudioManager.Instance.PlaySoudManager(1);
            UIManager.Instance.UpdateTextLevel();
        }
    }

    public void UpdateCurrentExp(int _exp)
    {
        CurrentExp += _exp;
        UpdateLevelProgress();
        UIManager.Instance.UpdateExpLevel();
    }

}
