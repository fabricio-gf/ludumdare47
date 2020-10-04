using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyState : MonoBehaviour
{
    [HideInInspector] public EnemyController _enemyController;
    public virtual void OnEnterState()
    {

    }

    public virtual void OnStateUpdate()
    {

    }

    public virtual void OnExitState()
    {

    }
}
