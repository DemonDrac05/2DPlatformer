using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkullBullet : EnemyAttackState
{
    float bulletDuration = 5f;

    public PlayerLife player;

    public SkullBullet(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
    {
    }

    void Start()
    {
        bulletDuration = 5f;
    }

    private void FixedUpdate()
    {
        player = FindObjectOfType<PlayerLife>();
        bulletDuration -= Time.deltaTime;
        if (bulletDuration <= 0f ) { Destroy(gameObject); }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            player.PlayerHealth -= 20f;
            Destroy(gameObject);
        }
    }
}
