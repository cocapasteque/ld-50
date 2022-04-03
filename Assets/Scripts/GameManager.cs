using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Player PlayerObj;
    public List<ObjectSpawner> Spawners;

    public float timer { get; private set; }
    public bool running { get; private set; }

    public static GameManager Instance;

    void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
        {
            Instance = this;
        }       
    }

    private void Start()
    {
        Init();
    }

    private void Update()
    {
        if (running)
            timer += Time.deltaTime;
    }

    public void Init()
    {
        timer = 0f;
        running = true;
        PlayerObj.Init();
        foreach(var spawner in Spawners)
        {
            spawner.Init();
        }
    }

    public void GameOver()
    {

    }
}
