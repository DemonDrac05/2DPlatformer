using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerLife : MonoBehaviour
{
    private PlayerMovement playerMovement;

    private Rigidbody2D rb2d;
    private Animator animator;
    private BoxCollider2D bc2d;

    [SerializeField] private AudioSource defeatSoundEffect;

    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        bc2d = GetComponent<BoxCollider2D>();
        playerMovement = GetComponent<PlayerMovement>();
    }
    private void Update()
    {
        if (!playerMovement.isDashing) { bc2d.isTrigger = false; rb2d.gravityScale = 3f; }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("trap"))
        {
            if (playerMovement.isDashing) { rb2d.gravityScale = 0f; bc2d.isTrigger = true; }
            else if (!playerMovement.isDashing) { defeatSoundEffect.Play(); Die(); }
        }
    }
    private void Die()
    {
        rb2d.bodyType = RigidbodyType2D.Static;
        animator.SetTrigger("death");
    }
    private void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
