using System;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private const string TRIGGER_EXPLODE = "explode";

    [SerializeField] private float speed;
    private float direction;
    private bool hit;
    private float lifetime;

    private Animator anim;
    private BoxCollider2D boxCollider;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        if (hit) return;
        float movementSpeed = speed * Time.deltaTime * direction;
        transform.Translate(movementSpeed, 0, 0);

        lifetime += Time.deltaTime;
        if (lifetime > 5) Deactivate();
    }

    private void OnTriggerEnter2D(Collider2D collison)
    {
        hit = true;
        boxCollider.enabled = false;
        anim.SetTrigger(TRIGGER_EXPLODE);
    }

    public void SetDirection(float direction)
    {
        lifetime = 0;
        this.direction = direction;
        gameObject.SetActive(true);
        hit = false;
        boxCollider.enabled = true;

        float localScaleX = transform.localScale.x;
        if (Mathf.Sign(localScaleX) != Mathf.Sign(direction))
            localScaleX = -localScaleX;

        transform.localScale = new Vector3(localScaleX, transform.localScale.y, transform.localScale.z);
    }

    private void Deactivate()
    {
        // print("Deactivate()");
        gameObject.SetActive(false);
    }
}
