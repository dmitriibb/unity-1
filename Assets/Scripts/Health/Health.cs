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

    private AudioClip soundHurt;
    private AudioClip soundDied;

    private void Awake()
    {
        currentHealth = startingHealth;
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        UpdateSounds();
    }

    private void UpdateSounds()
    {
        if (soundHurt == null && soundDied == null && GlobalVars.Instance != null)
        {
            soundHurt = GlobalVars.Instance.SoundHurt;
            soundDied = GlobalVars.Instance.SoundDied;
        }
    }

    public void TakeDamage(float damage)
    {
        UpdateSounds();
        if (invulnerabile) return;
        // print($"{tag}-TakeDamage({damage}) ({currentHealth}/{startingHealth})");
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
            SoundManager.instance.PlaySound(soundHurt);
            anim.SetTrigger(Constants.TRIGGER_PLAYER_HURT);
            StartCoroutine(Invulnerability());
        }
        else
        {
            SoundManager.instance.PlaySound(soundDied);
            // foreach (Behaviour component in components)
            // {
            //     component.enabled = false;
            // }
            anim.SetTrigger(Constants.TRIGGER_PLAYER_DIED);
            dead = true;
        }
    }

    private void TakeDamageEnemy(float damage)
    {
        if (dead) return;
        currentHealth = Mathf.Clamp(currentHealth - damage, 0, startingHealth);
        if (currentHealth > 0)
        {
            SoundManager.instance.PlaySound(soundHurt);
            anim.SetTrigger(Constants.TRIGGER_ENEMY_HURT);
        }
        else
        {
            SoundManager.instance.PlaySound(soundDied);
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

    public void RespwanHealth()
    {
        print($"RespwanHealth");
        dead = false;
        AddHealth(startingHealth);
        anim.ResetTrigger(Constants.TRIGGER_PLAYER_DIED);
        anim.Play(Constants.ANIM_PLAYER_IDLE);
        StartCoroutine(Invulnerability());

        foreach (Behaviour component in components)
            component.enabled = true;
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
        if (!CompareTag(Constants.TAG_PLAYER))
            gameObject.SetActive(false);
    }

}