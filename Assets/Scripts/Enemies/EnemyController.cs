﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Serialization;

public class EnemyController : MonoBehaviour
{
    
    [SerializeField] protected List<EnemyState> _allStates;
    public List<EnemyState> AllStates => _allStates;
    
    protected EnemyState _currentState;
    [SerializeField] protected float _detectionRange;
    public float DetectionRange => _detectionRange;

    [SerializeField] protected float _speed;
    public float Speed => _speed;
    
    [SerializeField] private AttackDefinition _attack;
    public AttackDefinition Attack => _attack;
    
    
    
    private Rigidbody2D _rb;
    public Rigidbody2D Rigidbody => _rb;

    private SpriteRenderer _spriteRenderer;
    public SpriteRenderer SpriteRenderer => _spriteRenderer;

    private Animator _animator;
    public Animator Animator => _animator;
    
    protected virtual void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _animator = GetComponentInChildren<Animator>();
            
        _currentState = _allStates[0];

        foreach (var state in _allStates)
        {
            state._enemyController = this;
        }
    }

    protected virtual void Update()
    {
        if (_currentState == null) return;
        _currentState.OnStateUpdate();
    }

    public virtual void ChangeState(EnemyState enemyState)
    {
        _currentState.OnExitState();
        _currentState = enemyState;
        _currentState.OnEnterState();
    }

    public virtual void FindNextState()
    {
        ChangeState(AllStates[(int)EnemyBlackboard.EnemyStates.idle]);
    }

    protected virtual void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, _detectionRange);
    }
    
    public bool PlayerOnDetectionRange()
    {
        var dist = Vector2.Distance(EnemyBlackboard.Instance.player.position, transform.position);
        return dist <= DetectionRange && dist >= 1.5f;
    }
    
    public bool PlayerOnAttackRange()
    {
        var dist = Vector2.Distance(EnemyBlackboard.Instance.player.position, transform.position);
        return dist <= _attack.AttackRange;
    }
}
