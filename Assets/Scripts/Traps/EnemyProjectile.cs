using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : EnemyDamage
{
    [SerializeField] private float speed;
    [SerializeField] private float resetTime;
    private float lifetime;
    private float direction;
    private Animator anim;
    private bool hit;

    private void Awake() {
        anim = GetComponent<Animator>();
    }

    public void ActiveteProjectile()
    {
        hit = false;
        lifetime = 0;
        gameObject.SetActive(true);
        direction = Mathf.Sign(Utils.GetPlayerPositionX() - transform.position.x);
        if (direction > 0)
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
    }

    private void Update()
    {
        if (!hit){
            float movementSpeed = speed * direction * Time.deltaTime;
            transform.Translate(movementSpeed, 0, 0);
        }

        lifetime += Time.deltaTime;
        if (lifetime > resetTime)
            gameObject.SetActive(false);
    }

    protected override void OnTriggerEnter2D(Collider2D collider)
    {
        base.OnTriggerEnter2D(collider);
        hit = true;
        if (anim != null)
            anim.SetTrigger(Constants.TRIGGER_FIREBALL_EXPLODE);
        else
            gameObject.SetActive(false);

        // if (collider.CompareTag(Constants.TAG_WALL)) 
    }

    private void Deactivate()
    {
        gameObject.SetActive(false);
    }

}
