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

    public Vector2 xScale;
    public Vector2 yScale;

    private Camera _cam;
    private bool _mouseDown = false;
    private Rigidbody2D _rb;
    private Vector3 _baseScale;
    private SpriteRenderer _sprite;

    private void Awake()
    {
        _cam = Camera.main;
        _rb = GetComponent<Rigidbody2D>();
        _baseScale = transform.localScale;
        _sprite = GetComponent<SpriteRenderer>();
    }

    public void Init()
    {
        _rb.velocity = Vector2.zero;
        transform.position = Vector2.zero;
    }

    private void ScaleStuff()
    {
        _sprite.flipX = _rb.velocity.x < 0;
        float test = Mathf.Abs(_rb.velocity.x) - Mathf.Abs(_rb.velocity.y);
        float scaleX = test > 0 ? Mathf.Lerp(1, xScale[0], test / xScale[1]) : 1f;
        float scaleY = test < 0 ? Mathf.Lerp(1, yScale[0], -test / yScale[1]) : 1f;
        
        transform.localScale = new Vector2(_baseScale.x * scaleX, _baseScale.y * scaleY);
    }

    void Update()
    {
        ScaleStuff();
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