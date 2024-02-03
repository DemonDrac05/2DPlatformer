using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DMSlashMovement : MonoBehaviour
{
    public float slashSpeed = 5f;

    private Rigidbody2D rb2d;

    private float slashDuration;

    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        slashDuration = 5f;
    }

    private void FixedUpdate()
    {
        rb2d.velocity = transform.right * slashSpeed;
        slashDuration -= Time.deltaTime;
        if(slashDuration < 0f) { Destroy(gameObject); }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("trap"))
        {
            Destroy(gameObject);
        }
    }
}
