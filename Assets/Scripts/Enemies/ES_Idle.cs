using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ES_Idle : EnemyState
{
    public override void OnEnterState()
    {
        base.OnEnterState();
    }

    public override void OnStateUpdate()
    {
        base.OnStateUpdate();
        
        if (_enemyController.PlayerOnDetectionRange())
        {
            _enemyController.ChangeState(_enemyController.AllStates[(int)EnemyBlackboard.EnemyStates.chase]);
        }
    }

    public override void OnExitState()
    {
        base.OnExitState();
    }
}
