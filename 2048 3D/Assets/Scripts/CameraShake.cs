using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake instance;
    public float power = 0.7f;
    public float duration = 1f;

    public new Transform camera;
    public float slowDownAmount = 1f;
    public bool shouldShake;

    Vector3 startPosition;
    float initialDuration;

    void Start()
    {
        instance = this;
        startPosition = camera.localPosition;
        initialDuration = duration;
    }

    void Update()
    {
        if (shouldShake)
        {
            if (duration > 0)
            {
                camera.localPosition = startPosition + Random.insideUnitSphere * power;
                duration -= Time.deltaTime * slowDownAmount;
            }
        }
        else
        {
            shouldShake = false;
            duration = initialDuration;
            camera.localPosition = startPosition;
        }
    }

    //public IEnumerator CamShake()
    //{
    //    while (true)
    //    {
    //        if (duration > 0)
    //        {
    //            camera.localPosition = startPosition + Random.insideUnitSphere * power;
    //            duration -= Time.deltaTime * slowDownAmount;
    //        }
    //        else
    //        {
    //            duration = initialDuration;
    //            camera.localPosition = startPosition;
    //        }
    //        yield return null;
    //    }
    //}
}
