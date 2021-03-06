using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Doozy.Engine;

public class GameManager : MonoBehaviour
{
    public Player PlayerObj;
    public List<ObjectSpawner> Spawners;

    public float timer { get; private set; }
    public bool running { get; private set; }

    [TextArea]
    public string PopText;
    [TextArea]
    public string FloorText;
    [TextArea]
    public string FloorHintText;

    public UIManager UIManager;
    public Text GameOverText;

    public static GameManager Instance;

    private Camera _cam;
    private float _regularCamSize = 6f;
    private const float _regularAspect = 16f / 9f;

    void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
        {
            Instance = this;
        }
        _cam = Camera.main;
        var currentAspect = (float)Screen.width / Screen.height;
        if (currentAspect < _regularAspect)
        {
            _cam.orthographicSize = (_regularAspect / currentAspect) * _regularCamSize;
        }
    }

    private void Update()
    {
        if (running)
            timer += Time.deltaTime;
    }

    public void Init()
    {
        StartCoroutine(DelayedInit());
        IEnumerator DelayedInit()
        {
            yield return new WaitForSeconds(0.2f);
            timer = 0f;
            running = true;
            PlayerObj.Init();
            foreach (var spawner in Spawners)
            {
                spawner.Init();
            }
            var enemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (var enemy in enemies)
            {
                Destroy(enemy);
            }
        }
    }

    public void GameOver(bool pop = false)
    {
        foreach (var spawner in Spawners)
        {
            spawner.Stop();
        }
        running = false;
        UIManager.UpdateTimer();
        GameEventMessage.SendEvent("GameOver");
        GameOverText.text = (pop ? PopText : FloorText) + (timer < 4f ? "\n" + FloorHintText : "") + $"\n\n You endured for {UIManager.TimerText.text} " + (timer > 4f ? "\n Good job!" : "");
        BoardEntry entry = new BoardEntry();
        entry.name = NameManager.Instance.Name;
        entry.score = timer;
        Leaderboard.SendScore(entry);
    }
}
