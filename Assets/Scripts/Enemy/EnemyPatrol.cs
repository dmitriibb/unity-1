using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [SerializeField] private Transform leftEdge;
    [SerializeField] private Transform rightEdge;
    [SerializeField] private Transform enemy;
    [SerializeField] private float speed;
    [SerializeField] private Animator anim;

    [SerializeField] private float idleTimeLimit;
    private float idleTime;


    private Vector3 initScale;
    private bool movingLeft;

    private void Awake()
    {
        initScale = enemy.localScale;
        movingLeft = enemy.localScale.x < 0;
    }

    private void Update() {
        if (movingLeft && enemy.position.x < leftEdge.position.x)
            ChangeDirection();
        if (!movingLeft && enemy.position.x > rightEdge.position.x)
            ChangeDirection();
        
        idleTime += Time.deltaTime;
        if (idleTime > idleTimeLimit)
            MoveInDirection(movingLeft ? -1 : 1);
    }

    private void ChangeDirection()
    {
        anim.SetBool(Constants.ANIM_PARAM_ENEMY_MOVING, false);
        idleTime = 0;
        movingLeft = !movingLeft;
    }

    private void MoveInDirection(int directonX)
    {
        anim.SetBool(Constants.ANIM_PARAM_ENEMY_MOVING, true);
        enemy.localScale = new Vector3(Mathf.Abs(initScale.x) * directonX, initScale.y, initScale.x);
        enemy.position = new Vector3(enemy.position.x + Time.deltaTime * directonX * speed, enemy.position.y, enemy.position.z);
    }
}
