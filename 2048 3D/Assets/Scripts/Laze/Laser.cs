using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters;
using System;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public GameObject FirePoint;
    public GameObject FirePoint_2;
    public Camera Cam;
    public float MaxLength;
    public GameObject[] Prefabs;

    private Ray RayMouse;
    private Vector3 direction;
    private Quaternion rotation;

    [Header("GUI")]
    private float windowDpi;

    private int Prefab;
    private GameObject Instance;
    private EGA_Laser LaserScript;

    //Double-click protection
    private float buttonSaver = 0f;

    void Start()
    {
        //LaserEndPoint = new Vector3(0, 0, 0);
        if (Screen.dpi < 1) windowDpi = 1;
        if (Screen.dpi < 200) windowDpi = 1;
        else windowDpi = Screen.dpi / 200f;
        LaserPoint();
    }

    void Update()
    {
        if (Cam != null)
        {
            RotateToMouseDirection(gameObject, FirePoint_2.transform.position);
        }
        else
        {
            Debug.Log("No camera");
        }
    }

    //To rotate fire point
    void RotateToMouseDirection(GameObject obj, Vector3 destination)
    {
        direction = destination - obj.transform.position;
        rotation = Quaternion.LookRotation(direction);
        obj.transform.localRotation = Quaternion.Lerp(FirePoint.transform.rotation, rotation, 1);
    }

    void LaserPoint()
    {
        for (int i = 0; i < MaxLength; i++)
        {
            //Enable lazer
            Instance = Instantiate(Prefabs[Prefab], FirePoint.transform.position, FirePoint.transform.rotation);
            Instance.transform.parent = transform;
            LaserScript = Instance.GetComponent<EGA_Laser>();
        }
    }
}
