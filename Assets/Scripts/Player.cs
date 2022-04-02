using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float GustStrength;
    public float GustFactor = 1f;
    public float AvailableGust = 1f;
    public float GustDrainFactor;
    public float GustRechargeFactor;


    private Camera _cam;
    private bool _mouseDown = false;
    private Rigidbody2D _rb;

    private void Awake()
    {
        _cam = Camera.main;
        _rb = GetComponent<Rigidbody2D>();
    }

    public void Init()
    {
        _rb.velocity = Vector2.zero;
        transform.position = Vector2.zero;
    }

    void Update()
    {       
        if (Input.GetMouseButtonDown(0))
        {           
            _mouseDown = true;
        }

        if (_mouseDown)
        {
            if (AvailableGust > 0)
            {
                GustStrength += Time.deltaTime;
                AvailableGust = Mathf.Clamp01(AvailableGust - GustDrainFactor * Time.deltaTime);
            }
        }
        else
        {
            AvailableGust = Mathf.Clamp01(AvailableGust + GustRechargeFactor * Time.deltaTime);
        }

        if (Input.GetMouseButtonUp(0))
        {
            _mouseDown = false;
            CastWind(_cam.ScreenToWorldPoint(Input.mousePosition));
            GustStrength = 0f;
        }        
    }

    public void CastWind(Vector2 pos)
    {
        float distance = Mathf.Clamp(Vector2.Distance(pos, transform.position), 0.5f, float.MaxValue);
        _rb.AddForce(((Vector2)transform.position - pos).normalized * GustStrength * GustFactor / distance, ForceMode2D.Impulse);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "Enemy":
                GameOver(true);
                break;
            default:
                Debug.Log(collision.tag);
                break;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Floor")
            GameOver();
    }

    private void GameOver(bool pop = false)
    {
        Debug.Log("Game Over");
        GameManager.Instance.GameOver();
    }
}