using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRanged : EnemyController
{
    [SerializeField] private float _attackCooldown;
    public float AttackCooldown => _attackCooldown + Attack.AttackDuration;

    [HideInInspector] public float _lastAttackTime;

    [SerializeField] private Transform _attackPivot;
    public Transform AttackPivot => _attackPivot;

    [SerializeField] private Transform[] _attackSprites;
    public Transform[] AttackSprites => _attackSprites;

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();

        if (PlayerOnAttackRange() && Mathf.Abs(transform.position.y - EnemyBlackboard.Instance.player.transform.position.y) <= 1.3f)
        {
            if (Time.unscaledTime >= _lastAttackTime + AttackCooldown) ChangeState(AllStates[(int)EnemyBlackboard.EnemyStates.fire]);
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
