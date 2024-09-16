using System.Collections;
using UnityEngine;

public class Firetrap : MonoBehaviour
{
    [SerializeField] private float damage;

    [Header("Firetrap Timers")]
    [SerializeField] private float activationDelay;
    [SerializeField] private float activeTime;  

    private Animator anim;
    private SpriteRenderer spriteRenderer;

    private bool triggered;
    private bool active;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag(Constants.TAG_PLAYER))
        {
            if (!triggered)
            {
                StartCoroutine(ActivateFiretrap());
            }
            if (active)
                collider.GetComponent<Health>().TakeDamage(damage);
        }

    }

    private IEnumerator ActivateFiretrap()
    {
        triggered = true;
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(activationDelay);
        
        spriteRenderer.color = Color.white;
        active = true;
        anim.SetBool(Constants.ANIM_PARAM_ACTIVATED, true);
        yield return new WaitForSeconds(activeTime);
        
        active = false;
        anim.SetBool(Constants.ANIM_PARAM_ACTIVATED, false);
        triggered = false;
    }

}
