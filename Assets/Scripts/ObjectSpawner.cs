using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ObjectSpawner : MonoBehaviour
{
    public List<SpawnPairs> Spawns;
    public List<GameObject> Prefabs;
    public AnimationCurve Cooldown;

    public void Init()
    {
        Spawn();
    }

    protected abstract void Spawn();

}
