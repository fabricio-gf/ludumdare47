using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAttack", menuName = "Attack Definition")]
[System.Serializable]
public class AttackDefinition : ScriptableObject
{
    [SerializeField] private AnimationClip _attackAnim;
    [SerializeField] private float _attackRange;
    [SerializeField] private float _attackDamage;

    public AnimationClip AttackAnim => _attackAnim;
    public float AttackDuration => _attackAnim.length;
    public float AttackRange => _attackRange;
    public float AttackDamage => _attackDamage;
}