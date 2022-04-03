using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SawbladeSpawner : ObjectSpawner
{
    protected override void Spawn()
    {
        StartCoroutine(SpawnObj());
    }

    private IEnumerator SpawnObj()
    {
        while (GameManager.Instance.running)
        {
            yield return new WaitForSeconds(Cooldown.Evaluate(GameManager.Instance.timer));

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
                obj.GetComponentInChildren<SpriteRenderer>().flipX = true;
            }
        }
    }
}
