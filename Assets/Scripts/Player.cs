using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int MaxLives = 2;

    public float GustStrength;
    public float GustFactor = 1f;
    public float AvailableGust = 1f;
    public float GustDrainFactor;
    public float GustRechargeFactor;   

    public Vector2 xScale;
    public Vector2 yScale;

    public List<Sprite> Sprites;
    public float InvincibilityDuration = 1f;

    private Camera _cam;
    private bool _mouseDown = false;
    private Rigidbody2D _rb;
    private Vector3 _baseScale;
    private SpriteRenderer _sprite;
    private int _lives;
    private bool _invincible;
    private Vector3 _baseGustScale;
    private float _gravity;
    private bool _gameOver;
    private Vector2 _startPos;

    public Animator GustAnim;
    public Transform GustEffect;
    public float GustScaleModifier;

    private bool _canUseMouse;

    private void Awake()
    {
        _cam = Camera.main;
        _rb = GetComponent<Rigidbody2D>();
        _baseScale = transform.localScale;
        _sprite = GetComponent<SpriteRenderer>();
        _baseGustScale = GustEffect.localScale;
        _gravity = _rb.gravityScale;
        _rb.gravityScale = 0f;
        _startPos = transform.position;
        _canUseMouse = Application.platform != RuntimePlatform.Android && Application.platform != RuntimePlatform.IPhonePlayer && Input.mousePresent;
    }

    public void Init()
    {
        _rb.gravityScale = _gravity;
        _rb.velocity = Vector2.zero;
        transform.position = _startPos;
        _lives = MaxLives;
        _sprite.sprite = Sprites[_lives];
        _invincible = false;
        _mouseDown = false;
        _gameOver = false;
        AvailableGust = 1f;
    }

    private void ScaleStuff()
    {
        _sprite.flipX = _rb.velocity.x > 0;
        float test = Mathf.Abs(_rb.velocity.x) - Mathf.Abs(_rb.velocity.y);
        float scaleX = test > 0 ? Mathf.Lerp(1, xScale[0], test / xScale[1]) : 1f;
        float scaleY = test < 0 ? Mathf.Lerp(1, yScale[0], -test / yScale[1]) : 1f;
        
        transform.localScale = new Vector2(_baseScale.x * scaleX, _baseScale.y * scaleY);
    }

    void Update()
    {
        if (GameManager.Instance.running)
        {
            ScaleStuff();

            if (_canUseMouse)
                UpdateWithMouse();
            else
                UpdateWithTouch();         
        }
    }

    private void UpdateWithMouse()
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

    private void UpdateWithTouch()
    {
     
        if (Input.touchCount > 0)
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

        if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Ended)
        {
            _mouseDown = false;
            CastWind(_cam.ScreenToWorldPoint(Input.touches[0].position));
            GustStrength = 0f;
        }
    }

    public void CastWind(Vector2 pos)
    {
        float distance = Mathf.Clamp(Vector2.Distance(pos, transform.position), 0.5f, float.MaxValue);
        _rb.AddForce(((Vector2)transform.position - pos).normalized * GustStrength * GustFactor / distance, ForceMode2D.Impulse);
        
        GustAnim.SetTrigger("Gust");
        GustEffect.position = pos;

        Vector2 diff = ((Vector2)transform.position - pos);
        float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        GustEffect.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);

        GustEffect.localScale = _baseGustScale * (1 + (GustScaleModifier * GustStrength)); 
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "Enemy":
                if (!_invincible)
                    LoseLife();
                break;
            default:
                Debug.Log(collision.tag);
                break;
        }
    }

    private void LoseLife()
    {
        StartCoroutine(Invincibility());
        _lives--;
        if (_lives >= 0)
        {
            _sprite.sprite = Sprites[_lives];
        }
        else if (!_gameOver)
        {
            _sprite.sprite = null;
            GameOver(true);
        }
    }

    private IEnumerator Invincibility()
    {
        _invincible = true;
        yield return new WaitForSeconds(InvincibilityDuration);
        _invincible = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Floor" && !_gameOver)
            GameOver(false);
    }

    private void GameOver (bool pop)
    {
        _gameOver = true;
        GameManager.Instance.GameOver(pop);
        _invincible = true;
    }
}