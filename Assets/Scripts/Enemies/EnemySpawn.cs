using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemySpawn : MonoBehaviour
{
    public GameObject enemyPrefab;

    private void Awake()
    {
        //EnemyBlackboard.Instance.Initialize();
    }

    public void SpawnEnemy()
    {
        GameObject newEnemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity);
        newEnemy.transform.localScale = Vector3.zero;
        newEnemy.transform.DOScale(Vector3.one, 0.12f);
    }
}
