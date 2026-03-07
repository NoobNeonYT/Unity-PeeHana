using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : MonoBehaviour
{
    [Header("Movement")]
    public float speed;
    public float distance;
    private bool movingRight = true;
    public Transform groundDetection;

    [Header("Animation Setup")]
    public Animator enemyAnimator;

    void Update()
    {
        EnemyMovement();
        EnemyInput();
    }

    void EnemyMovement()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);
        RaycastHit2D groundInfo = Physics2D.Raycast(groundDetection.position, Vector2.down, distance);

        if (groundInfo.collider == false)
        {
            if (movingRight == true)
            {
                transform.eulerAngles = new Vector3(0, -180, 0);
                movingRight = false;
            }
            else
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
                movingRight = true;
            }
        }
    }

    void EnemyInput()
    {
        if (enemyAnimator != null)
        {
            if (Input.GetKeyDown(KeyCode.R)) enemyAnimator.SetTrigger("EnemyDeath");
            if (Input.GetKeyDown(KeyCode.T)) enemyAnimator.SetTrigger("EnemyHurt");
            if (Input.GetKeyDown(KeyCode.Y)) enemyAnimator.SetTrigger("EnemyRun");
        }
    }
}