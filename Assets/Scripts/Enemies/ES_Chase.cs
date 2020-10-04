using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ES_Chase : EnemyState
{
    public override void OnEnterState()
    {
        base.OnEnterState();
        _enemyController.Animator.SetBool("Moving", true);
    }

    public override void OnStateUpdate()
    {
        base.OnStateUpdate();
        
        if (!_enemyController.PlayerOnDetectionRange()) _enemyController.ChangeState(_enemyController.AllStates[(int)EnemyBlackboard.EnemyStates.idle]);

        Vector2 dir = ((Vector2)_playerTransform.position - _enemyController.Rigidbody.position).normalized;
        
        if (dir.x < 0)
        {
            _enemyController.SpriteRenderer.flipX = true;
        }
        else if (dir.x > 0)
        {
            _enemyController.SpriteRenderer.flipX = false;
        }
        
        _enemyController.Rigidbody.MovePosition(_enemyController.Rigidbody.position + dir * (_enemyController.Speed * Time.fixedDeltaTime));
    }

    public override void OnExitState()
    {
        base.OnExitState();
        _enemyController.Animator.SetBool("Moving", false);
    }
}
