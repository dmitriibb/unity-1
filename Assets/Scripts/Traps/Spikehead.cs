using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Spikehead : EnemyDamage
{
    [SerializeField] private float speed;
    [SerializeField] private float range;
    [SerializeField] private float checkDelay;
    [SerializeField] private LayerMask playerLayer;
    private float checkTimer;
    private Vector3 destination;

    private bool attacking;
    private Vector3[] directions = new Vector3[4];

    private void OnEnable()
    {
        Stop();
    }
    
    private void Update()
    {
        if (attacking)
            transform.Translate(speed * Time.deltaTime * destination);
        else
        {
            checkTimer += Time.deltaTime;
            if (checkTimer > checkDelay)
                CheckForPlayer();
        }
    }

    private void CheckForPlayer()
    {
        CalculateDirections();
        foreach (Vector3 direction in directions)
        {
            Debug.DrawRay(transform.position, direction, Color.red);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, range, playerLayer);

            if (hit.collider != null && !attacking)
            {
                attacking = true;
                destination = direction;
                checkTimer = 0;
            }
        }
    }

    private void CalculateDirections()
    {
        directions[0] = transform.right * range;
        directions[1] = -transform.right * range;
        directions[2] = transform.up * range;
        directions[3] = -transform.up * range;
    }

    private void Stop()
    {
        destination = transform.position;
        attacking = false;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        base.OnTriggerEnter2D(collider);
        Stop();
    }
}
