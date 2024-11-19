using UnityEngine;

public class DestroyOnDeath : MonoBehaviour
{
    HealthSystem healthSystem;
    Rigidbody2D rb;

    void Start()
    {
        healthSystem = GetComponent<HealthSystem>();
        rb = GetComponent<Rigidbody2D>();

        healthSystem.OnDeath += OnDeath;
    }

    void OnDeath()
    {
        rb.velocity = Vector2.zero;

        foreach(SpriteRenderer renderer in GetComponentsInChildren<SpriteRenderer>())
        {
            Color col = renderer.color;

            col.a = 0.3f;

            renderer.color = col;
        }

        foreach (Behaviour be in GetComponentsInChildren<Behaviour>())
        {
            be.enabled = false;
        }

        Destroy(gameObject, 2f);
    }

}