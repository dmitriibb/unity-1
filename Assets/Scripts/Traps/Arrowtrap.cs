using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrowtrap : MonoBehaviour
{
    [SerializeField] private float attackCooldown;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject[] arrows;
    private float cooldowdTimer;

    private void Attack()
    {
        cooldowdTimer = 0;
        GameObject arrow = Utils.PullInactiveGameObjectFromList(arrows);
        arrow.transform.position = firePoint.position;
        arrow.GetComponent<EnemyProjectile>().ActiveteProjectile();
            
    }

    private void Update()
    {
        cooldowdTimer += Time.deltaTime;
        if (cooldowdTimer >= attackCooldown)
            Attack();
    }
}
