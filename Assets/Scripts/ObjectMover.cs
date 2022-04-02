using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMover : MonoBehaviour
{
    public float Speed;
    public AnimationCurve AccelerationCurve;
    public float AccelerationDuration;

    private float startTime;

    private void OnEnable()
    {
        startTime = Time.time;
    }

    private void Update()
    {
        transform.Translate(Vector3.right * AccelerationCurve.Evaluate((Time.time - startTime) / AccelerationDuration) * Speed * Time.deltaTime, Space.Self);
    }
}