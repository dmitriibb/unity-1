using UnityEngine;

public class MeleeEnemy : MonoBehaviour
{
    [SerializeField] private float attackCooldown;
    [SerializeField] private float range;
    [SerializeField] private int damage;
    [SerializeField] private AudioClip soundAttack;
    
    [SerializeField] private float colliderDistance;
    [SerializeField] private BoxCollider2D boxCollider2;

    [SerializeField] private LayerMask playerLayer;
    private float cooldownTimer = Mathf.Infinity;

    private Animator anim;
    private Health playerHealth;

    private void Awake()
    {
        // boxCollider2 = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        cooldownTimer += Time.deltaTime;
        if (IsPlayerInSight())
        {
            if(cooldownTimer >= attackCooldown && playerHealth.currentHealth > 0)
            {
                cooldownTimer = 0;
                SoundManager.instance.PlaySound(soundAttack);
                anim.SetTrigger(Constants.TRIGGER_ENEMY_ATTACK_MELEE);
            }
        }  
    }

    private bool IsPlayerInSight()
    {
        RaycastHit2D hit = Physics2D.BoxCast(
            boxCollider2.bounds.center + range * transform.localScale.x * colliderDistance * transform.right, 
            RaycastSize(), 0, Vector2.left, 0, playerLayer);
        if (hit.collider != null && hit.collider.CompareTag(Constants.TAG_PLAYER))
        {
            playerHealth = hit.transform.GetComponent<Health>();
            return true;
        }
        return false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider2.bounds.center + range * transform.localScale.x * transform.right, RaycastSize());
    }

    private Vector2 RaycastSize()
    {
        return new Vector2(boxCollider2.bounds.size.x * colliderDistance * range, boxCollider2.bounds.size.y);
    }

    private void OnCollisionEnter2D(Collision2D other) {
        print($"OnCollisionEnter2D-{other.collider.name}");
    }

    private void OnTriggerEnter2D(Collider2D other) {
        print($"OnTriggerEnter2D-{other.gameObject.name}");
    }

    // invoked by animation event
    private void DamagePlayer()
    {
        if (IsPlayerInSight())
            playerHealth.TakeDamage(damage);
    }

}
