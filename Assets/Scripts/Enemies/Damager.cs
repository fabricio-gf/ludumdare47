using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Damager : MonoBehaviour
{
    private Character2DController PlayerController => EnemyBlackboard.Instance._playerController;

    public enum DamagerType
    {
        player,
        enemy
    }

    [SerializeField] private DamagerType damagerType;

    private EnemyController _enemyController;

    private void Start()
    {
        switch (damagerType)
        {
            case DamagerType.enemy:
                _enemyController = GetComponentInParent<EnemyController>();
                break;
            case DamagerType.player:
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        switch (damagerType)
        {
            case DamagerType.enemy:
                if (other.CompareTag("Player")) PlayerController.TakeDamage(_enemyController.Attack.AttackDamage);
                break;
            case DamagerType.player:
                break;
        }
    }
}