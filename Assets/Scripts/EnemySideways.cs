using UnityEngine;

public class EnemySideways : MonoBehaviour
{
    [SerializeField] private float movementDistance;
    [SerializeField] private float speed;
    [SerializeField] private float damage;
    private bool movingLeft;
    private float leftEdge;
    private float rightEdge;

    private void Awake()
    {
        leftEdge = transform.position.x - movementDistance;
        rightEdge = transform.position.x + movementDistance;
    }

    private void Update()
    {
        if (movingLeft && transform.position.x < leftEdge)
            movingLeft = false;
        if (!movingLeft && transform.position.x > rightEdge)
            movingLeft = true;

        float movementX = movingLeft 
        ? transform.position.x - speed * Time.deltaTime 
        : transform.position.x + speed * Time.deltaTime;
        transform.position = new Vector3(movementX, transform.position.y, transform.position.z);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == Constants.TAG_PLAYER)
        {
            collider.GetComponent<Health>().TakeDamage(damage);
        }
    }

}
