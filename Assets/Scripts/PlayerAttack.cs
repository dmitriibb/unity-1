
using System;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
   private const string TRIGGER_ATTACK = "attack";

   [SerializeField] private float attackCooldown;
   [SerializeField] private Transform firePoint;
   [SerializeField] private GameObject[] fireballs;
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
        anim.SetTrigger(TRIGGER_ATTACK);
        cooldownTimer = 0;

        fireballs[FindFireball()].transform.position = firePoint.position;
        fireballs[FindFireball()].GetComponent<Projectile>().SetDirection(Mathf.Sign(transform.localScale.x));
    }

    private int FindFireball()
    {
        for (int i = 0; i < fireballs.Length; i++)
        {
            if (!fireballs[i].activeInHierarchy)
                return i;
        }
        return 0;
    }
}
