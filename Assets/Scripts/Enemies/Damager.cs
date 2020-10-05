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
                if (other.CompareTag("Enemy"))
                {
                    float multiply;
                    if (UpgradesManager.Instance.flamingMuffin)
                        multiply = 2f;
                    else
                        multiply = 1f;
                    other.GetComponentInParent<EnemyController>().TakeDamage(1f * multiply);
                }
                if (other.CompareTag("Balloon")) other.GetComponent<Balloon>().PopBalloon();
                break;
        }
    }
}