using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRanged : EnemyController
{
    [SerializeField] private float _attackCooldown;
    public float AttackCooldown => _attackCooldown + Attack.AttackDuration;

    [HideInInspector] public float _lastAttackTime;

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();

        if (PlayerOnAttackRange())
        {
            if (Time.unscaledTime >= _lastAttackTime + AttackCooldown) ChangeState(AllStates[(int)EnemyBlackboard.EnemyStates.attack]);
        }
    }

    public override void FindNextState()
    {
        base.FindNextState();
    }

    protected override void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();
        
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, Attack.AttackRange);
    }
}
