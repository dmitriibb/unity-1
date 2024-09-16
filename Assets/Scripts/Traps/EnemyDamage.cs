using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    [SerializeField] protected float damage;

    protected virtual void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag(Constants.TAG_PLAYER))
            collider.GetComponent<Health>().TakeDamage(damage);
    }
}
