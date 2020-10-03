using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBlackboard : MonoBehaviour
{
    public static EnemyBlackboard instance;

    [HideInInspector] public Transform player;
    [HideInInspector] public int enemyCount;

    private void Awake()
    {
        EnemyBlackboard.instance = this;
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }
}
