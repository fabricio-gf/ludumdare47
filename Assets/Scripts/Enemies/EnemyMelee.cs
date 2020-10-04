using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMelee : EnemyController
{
    [SerializeField] private AttackDefinition _attack;
    public AttackDefinition Attack => _attack;

    [SerializeField] private float _attackCooldown;
    public float AttackCooldown => _attackCooldown + _attack.AttackDuration;

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
        Gizmos.DrawWireSphere(transform.position, _attack.AttackRange);
    }
    
    public bool PlayerOnAttackRange()
    {
        var dist = Vector2.Distance(EnemyBlackboard.Instance.player.position, transform.position);
        return dist <= _attack.AttackRange;
    }
}
