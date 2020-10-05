using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
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
        
        _enemyController.HandleFlip();

        float multiply;
        if (UpgradesManager.Instance.expiredMedicine)
            multiply = 0.5f;
        else
            multiply = 1f;
        _enemyController.Rigidbody.MovePosition(_enemyController.Rigidbody.position + _enemyController.PlayerDir * (_enemyController.Speed * multiply * Time.fixedDeltaTime));
    }

    public override void OnExitState()
    {
        base.OnExitState();
        _enemyController.Animator.SetBool("Moving", false);
    }
}
