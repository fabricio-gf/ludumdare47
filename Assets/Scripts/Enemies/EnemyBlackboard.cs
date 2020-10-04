using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBlackboard : MonoBehaviour
{
    private static EnemyBlackboard _instance;

    public static EnemyBlackboard Instance
    {
        get
        {
            if (_instance == null)
            {
                var enemyBlackboard = new GameObject("EnemyBlackboard");
                var newInstance = enemyBlackboard.AddComponent<EnemyBlackboard>();
                _instance = newInstance;
            }
            return _instance;
        }
    }

    [HideInInspector] public Transform _player;
    [HideInInspector] public Character2DController _playerController;
    [HideInInspector] public int enemyCount;

    public enum EnemyStates
    {
        idle = 0, chase = 1, attack = 2, fire = 3,
    }

    private void OnEnable()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _playerController = _player.GetComponent<Character2DController>();
    }
}
