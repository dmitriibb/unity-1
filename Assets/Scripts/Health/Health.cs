using System.Collections;
using UnityEngine;

public class Health : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private float startingHealth;
    public float currentHealth { get; private set; }
    private Animator anim;
    private bool dead;

    [Header("iFrames")]
    private bool invulnerabile;
    [SerializeField] private float iFramesDuration;
    [SerializeField] private int numberOfFlashes;
    private SpriteRenderer spriteRenderer;

    [Header("Components to disable after death")]
    [SerializeField] private Behaviour[] components;

    private void Awake()
    {
        currentHealth = startingHealth;
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void TakeDamage(float damage)
    {
        if (invulnerabile) return;
        print($"{tag}-TakeDamage({damage}) ({currentHealth}/{startingHealth})");
        switch (tag)
        {
            case Constants.TAG_PLAYER:
                TakeDamagePlayer(damage);
                break;
            case Constants.TAG_ENEMY:
                TakeDamageEnemy(damage);
                break;
            default:
                Debug.LogError($"TakeDamage - unklnown tag {tag}");
                break;
        }
    }

    private void TakeDamagePlayer(float damage)
    {
        if (dead) return;
        currentHealth = Mathf.Clamp(currentHealth - damage, 0, startingHealth);
        if (currentHealth > 0)
        {
            anim.SetTrigger(Constants.TRIGGER_PLAYER_HURT);
            StartCoroutine(Invulnerability());
        }
        else
        {
            anim.SetTrigger(Constants.TRIGGER_PLAYER_DIED);
            foreach (Behaviour component in components)
            {
                component.enabled = false;
            }
            dead = true;
        }
    }

    private void TakeDamageEnemy(float damage)
    {
        if (dead) return;
        currentHealth = Mathf.Clamp(currentHealth - damage, 0, startingHealth);
        if (currentHealth > 0)
        {
            anim.SetTrigger(Constants.TRIGGER_ENEMY_HURT);
        }
        else
        {

            anim.SetTrigger(Constants.TRIGGER_ENEMY_DIED);
            foreach (Behaviour component in components)
            {
                component.enabled = false;
            }
            print("disable MeleeEnemy");
            // Destroy(GetComponent<Rigidbody2D>());
            dead = true;

        }
    }

    public void AddHealth(float value)
    {
        currentHealth = Mathf.Clamp(currentHealth + value, 0, startingHealth);
    }

    
    private IEnumerator Invulnerability()
    {
        invulnerabile = true;
        Physics2D.IgnoreLayerCollision(Constants.LAYER_PLAYER, Constants.LAYER_ENEMY, true);
        for (int i = 0; i < numberOfFlashes; i++)
        {
            spriteRenderer.color = new Color(1, 0, 0, 0.5f);
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));
            spriteRenderer.color = Color.white;
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));
        }
        Physics2D.IgnoreLayerCollision(Constants.LAYER_PLAYER, Constants.LAYER_ENEMY, false);
        invulnerabile = false;
    }

    public void Died()
    {
        gameObject.SetActive(false);
    }

}