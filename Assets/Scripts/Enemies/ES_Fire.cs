using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ES_Fire : EnemyState
{
    private EnemyRanged _enemyRanged => _enemyController as EnemyRanged;
    private AttackDefinition _attack => _enemyRanged.Attack;

    private float _startTime;
    
    public override void OnEnterState()
    {
        base.OnEnterState();

        _enemyRanged._lastAttackTime = Time.unscaledTime;
        
        if (_attack.AttackAnim != null) _enemyController.Animator.Play(_attack.AttackAnim.name);
        _startTime = Time.unscaledTime;
        
        _enemyController.HandleFlip();
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
