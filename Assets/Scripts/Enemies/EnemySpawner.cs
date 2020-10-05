using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner Instance;
    
    private EnemySpawn[] _enemySpawns;

    private float time;

    public bool canSpawn = false;

    private void Awake()
    {
        print("My game object is " + gameObject.name);
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        _enemySpawns = FindObjectsOfType<EnemySpawn>();
    }

    private void Update()
    {
        if (!canSpawn) return;
        
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
