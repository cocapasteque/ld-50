using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRotator : MonoBehaviour
{
    public float RotationSpeed;

    private void Start()
    {
        RotationSpeed = RotationSpeed * (GetComponent<SpriteRenderer>().flipX ? 1f : -1f);
    }

    private void Update()
    {
        transform.Rotate(Vector3.forward * RotationSpeed * Time.deltaTime);
    }
}