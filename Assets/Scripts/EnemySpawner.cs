using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    private EnemySpawn[] _enemySpawns;

    private float time;

    private void Start()
    {
        _enemySpawns = FindObjectsOfType<EnemySpawn>();
    }

    private void Update()
    {
        if (time <= 0)
        {
            for (int i = 0; i < _enemySpawns.Length; i++)
            {
                var spawn = _enemySpawns[i];
                if (spawn.transform.position.x > EnemyBlackboard.Instance._player.transform.position.x &&
                    (spawn.transform.position.x - EnemyBlackboard.Instance._player.transform.position.x) <= 20f)
                {
                    spawn.SpawnEnemy();
                }
            }

            time = Random.Range(3f, 8f);
        }
        else
        {
            time -= Time.deltaTime;
        }
    }
}
