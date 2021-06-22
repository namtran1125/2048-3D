using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Ball : MonoBehaviour
{
    static int staticID = 0;
    [SerializeField] TMP_Text numbersTxt;

    [HideInInspector] public int BallID;
    [HideInInspector] public Color BallColor;
    [HideInInspector] public int BallNumber;
    //[HideInInspector] 
    public int ValueRandom;
    [HideInInspector] public float BallSize;
    [HideInInspector] public Rigidbody BallRb;
    [HideInInspector] public bool IsMainBall;
    [HideInInspector] public bool IsQueueCollision;
    [HideInInspector] public bool WhiteBall;

    [HideInInspector] public GameObject parentTxt;
    MeshRenderer ballMeshRenderer;
    new MeshCollider collider;

    [Header("<<-- Effect -->>")]
    public ParticleSystem Light;
    private void Awake()
    {
        BallID = staticID++;
        ballMeshRenderer = GetComponent<MeshRenderer>();
        BallRb = GetComponent<Rigidbody>();
        collider = GetComponent<MeshCollider>();

        parentTxt = transform.GetChild(0).gameObject;
        parentTxt.name = BallID.ToString();
    }
    void Start()
    {
        parentTxt.transform.parent = GameManager.Instance.ParentTxt.transform;
    }

    private void Update()
    {
        if(parentTxt != null)
        {
            parentTxt.transform.position = transform.position;
            parentTxt.transform.rotation = Quaternion.identity;
        }

        //if (BallRb.velocity.magnitude >= 7f)
        //    BallRb.velocity = Vector3.ClampMagnitude(BallRb.velocity, 7f);

        //Physics.gravity = new Vector3(0, GameManager.Instance.GravityBall, 0);
    }

    public void SetColor(Color _color)
    {
        BallColor = _color;
        ballMeshRenderer.material.color = _color;
    }

    public void SetNumber(int _num, int _val)
    {
        ValueRandom = _val;
        BallNumber = _num;
        if(_num == 0)
            numbersTxt.text = "";
        else
            numbersTxt.text = _num.ToString();

        //float sizeTest = (_num - (0.5f * _num / (_num + 0.5f * _num))) / 20f;

        //float sizeBall = ((1.25f / 2f) + ((_num / 2f) / 100f));
        //transform.localScale = Vector3.one * sizeBall;
    }

    public void SetSize(float _size)
    {
        BallSize = _size;
        transform.localScale = Vector3.one * _size;
    }
    public void SetCollider(bool _col)
    {
        collider.enabled = _col;
    }

}
