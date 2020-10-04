using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ES_Attack : EnemyState
{
    private EnemyMelee _enemyMelee => _enemyController as EnemyMelee;
    private AttackDefinition _attack => _enemyMelee.Attack;

    private float _startTime;
    
    public override void OnEnterState()
    {
        base.OnEnterState();

        _enemyMelee._lastAttackTime = Time.unscaledTime;
        
        if (_attack.AttackAnim != null) _enemyController.Animator.Play(_attack.AttackAnim.name);
        _startTime = Time.unscaledTime;
    }

    public override void OnStateUpdate()
    {
        base.OnStateUpdate();

        if (Time.unscaledTime >= _startTime + _attack.AttackDuration)
        {
            _enemyController.FindNextState();
        }
    }

    public override void OnExitState()
    {
        base.OnExitState();
    }
}
