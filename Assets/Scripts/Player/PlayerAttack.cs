
using System;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
   

   [SerializeField] private float attackCooldown;
   [SerializeField] private Transform firePoint;
   [SerializeField] private GameObject[] fireballs;
   [SerializeField] private AudioClip soundAttack;

   private float cooldownTimer = Mathf.Infinity;
   private Animator anim;
   private PlayerMovement playerMovement;

   private void Awake()
   {
        anim = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
   }

   private void Update()
   {
        if (Input.GetMouseButton(0) && cooldownTimer > attackCooldown && playerMovement.CanAttack())
            Attack();
        
        cooldownTimer += Time.deltaTime;
   }

    private void Attack()
    {
        SoundManager.instance.PlaySound(soundAttack);
        anim.SetTrigger(Constants.TRIGGER_PLAYER_ATTACK);
        cooldownTimer = 0;

        GameObject fireball = Utils.PullInactiveGameObjectFromList(fireballs);
        fireball.transform.position = firePoint.position;
        fireball.GetComponent<Projectile>().SetDirection(Mathf.Sign(transform.localScale.x));
    }

    // private int FindFireball()
    // {
    //     for (int i = 0; i < fireballs.Length; i++)
    //     {
    //         if (!fireballs[i].activeInHierarchy)
    //             return i;
    //     }
    //     return 0;
    // }
}
