using UnityEngine;

public class RangedEnemy : MonoBehaviour
{
    [SerializeField] private float attackCooldown;
    [SerializeField] private float range;
    [SerializeField] private int damage;

    [SerializeField] private Transform firepoint;
    [SerializeField] private GameObject[] fireballs;
    [SerializeField] private AudioClip soundAttack;

    [SerializeField] private float colliderDistance;
    [SerializeField] private BoxCollider2D boxCollider;

    [SerializeField] private LayerMask playerLayer;
    private float cooldownTimer = Mathf.Infinity;

    private Animator anim;

    private void Awake()
    {
        // boxCollider = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        cooldownTimer += Time.deltaTime;
        if (IsPlayerInSight() && cooldownTimer >= attackCooldown)
        {
            RangeAttack();
        }
            
    }

    private bool IsPlayerInSight()
    {
        RaycastHit2D hit = Physics2D.BoxCast(
            boxCollider.bounds.center + range * transform.localScale.x  * transform.right, 
            RaycastSize(), 0, Vector2.left, 0, playerLayer);
        return hit.collider != null && hit.collider.CompareTag(Constants.TAG_PLAYER);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider.bounds.center + range * transform.localScale.x * transform.right, RaycastSize());
    }

    private Vector2 RaycastSize()
    {
        return new Vector2(boxCollider.bounds.size.x * colliderDistance * range, boxCollider.bounds.size.y);
    }

    private void RangeAttack()
    {
        SoundManager.instance.PlaySound(soundAttack);
        anim.SetTrigger(Constants.TRIGGER_ENEMY_ATTACK_RANGE);
        GameObject fireball = Utils.PullInactiveGameObjectFromList(fireballs);
        print($"cooldownTimer={cooldownTimer}, attackCooldown = {attackCooldown}");
        cooldownTimer = 0;
        print($"{this}-{this.GetInstanceID()}RangeAttack() fireball - {fireball.GetInstanceID()}");
        fireball.transform.position = firepoint.position;
        fireball.GetComponent<EnemyProjectile>().ActiveteProjectile();
    }

}
