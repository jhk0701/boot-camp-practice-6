using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Fire(float speed, Vector2 dir)
    {
        rb.velocity = speed * dir;
    }

    public void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag.Equals("Obstacle"))
        {
            Destroy(gameObject);
        }
    }
}
