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
    }

    public override void OnStateUpdate()
    {
        base.OnStateUpdate();
        
        Vector2 dir = ((Vector2)_playerTransform.position - _enemyController.Rigidbody.position).normalized;
        float flip = dir.x < 0 ? 1 : -1;
        var spriteRendererTransform = _enemyController.SpriteRenderer.transform;
        spriteRendererTransform.localScale = new Vector2(flip, spriteRendererTransform.localScale.y);

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
