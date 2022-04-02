using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotTest : MonoBehaviour
{
    public Transform Target;

    private float _angle;

    // Update is called once per frame
    void Update()
    {
        _angle = Vector2.SignedAngle(Vector2.right, Target.position - transform.position);
        transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, _angle));
    }
}
