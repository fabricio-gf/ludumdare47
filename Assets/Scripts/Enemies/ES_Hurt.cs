using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ES_Hurt : EnemyState
{

    private float _stunTime = 0.3f;
    private float _time = 0f;
    
    public override void OnEnterState()
    {
        base.OnEnterState();
        _enemyController.Animator.SetBool("Hurt", true);
    }

    public override void OnStateUpdate()
    {
        base.OnStateUpdate();

        if (_time < _stunTime)
        {
            _time += Time.deltaTime;
        }
        else
        {
            _enemyController.FindNextState();
        }
    }

    public override void OnExitState()
    {
        base.OnExitState();
        _enemyController.Animator.SetBool("Hurt", false);
    }
}
