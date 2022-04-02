using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public List<SpawnPairs> Spawns;
    public List<GameObject> Prefabs;
    public float Cooldown;

    private float _timer = 0f;

    void Update()
    {
        if (GameManager.Instance.running)
        {
            _timer += Time.deltaTime;
            if (_timer > Cooldown)
            {
                SpawnObj();
                _timer = 0f;
            }
        }
    }

    private void SpawnObj()
    {
        int spawnPairIndex = Random.Range(0, Spawns.Count);
        var spawnSide = Random.Range(0f, 1f);
        Vector2 spawnPos = spawnSide > 0.5 ? Vector2.Lerp(Spawns[spawnPairIndex].Min1.position, Spawns[spawnPairIndex].Max1.position, Random.Range(0f, 1f)) :
            Vector2.Lerp(Spawns[spawnPairIndex].Min2.position, Spawns[spawnPairIndex].Max2.position, Random.Range(0f, 1f));
        Vector2 targetPos = spawnSide <= 0.5 ? Vector2.Lerp(Spawns[spawnPairIndex].Min1.position, Spawns[spawnPairIndex].Max1.position, Random.Range(0f, 1f)) :
            Vector2.Lerp(Spawns[spawnPairIndex].Min2.position, Spawns[spawnPairIndex].Max2.position, Random.Range(0f, 1f));

        Quaternion spawnRot = Quaternion.Euler(new Vector3(0f, 0f, Vector2.SignedAngle(Vector2.right, targetPos - spawnPos)));

        var obj = Instantiate(Prefabs[Random.Range(0, Prefabs.Count)], spawnPos, spawnRot);

        if (spawnPos.x > targetPos.x)
        {
            obj.GetComponent<SpriteRenderer>().flipY = true;
        }
    }
}
