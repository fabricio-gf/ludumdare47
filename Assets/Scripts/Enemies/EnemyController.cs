using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemyController
{ 
    public enum EnemyStates
    {
        idle = 0,
    }

    private EnemyState _currentState;
    [SerializeField] private List<EnemyState> _allStates;

    public void Start()
    {
        _currentState = _allStates[0];

        foreach (var state in _allStates)
        {
            state.enemyController = this;
        }
    }

    private void Update()
    {
        if (_currentState != null) _currentState.OnStateUpdate();
    }

    private void ChangeState(EnemyState enemyState)
    {
        _currentState = enemyState;
        _currentState.OnEnterState();
    }

    public void FindNextState()
    {
        
    }
}
