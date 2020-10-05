using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;


public class ES_Hurt : EnemyState
{
    private float _time = 0f;
    
    public override void OnEnterState()
    {
        base.OnEnterState();
        _enemyController.Animator.SetBool("Hurt", true);
        _enemyController.SpriteRenderer.DOColor(Color.red, 0.15f).SetLoops(2, LoopType.Yoyo);
    }

    public override void OnStateUpdate()
    {
        base.OnStateUpdate();

        if (_time < _enemyController.StunTime)
        {
            _time += Time.deltaTime;
        }
        else
        {
            _time = 0;
            _enemyController.FindNextState();
        }
    }

    public override void OnExitState()
    {
        base.OnExitState();
        _enemyController.Animator.SetBool("Hurt", false);
    }
}
