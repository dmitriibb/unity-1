using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float startingHealth;
    public float currentHealth { get; private set; }
    private Animator anim;
    private bool dead;

    private void Awake()
    {
        currentHealth = startingHealth;
        anim = GetComponent<Animator>();
    }

    public void TakeDamage(float damage)
    {
        currentHealth = Mathf.Clamp(currentHealth - damage, 0, startingHealth);
        if (currentHealth > 0)
        {
            anim.SetTrigger(Constants.TRIGGER_PLAYER_HURT);
        }
        else
        {
            if (!dead)
            {
                anim.SetTrigger(Constants.TRIGGER_PLAYER_DIED);
                GetComponent<PlayerMovement>().enabled = false;
                dead = true;
            }
        }
    }

    public void AddHealth(float value)
    {
        currentHealth = Mathf.Clamp(currentHealth + value, 0, startingHealth);
    }

    // private void Update()
    // {
    //     if(Input.GetKeyDown(KeyCode.E))
    //         TakeDamage(1);
    // }
}
