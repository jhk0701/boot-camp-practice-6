using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    [SerializeField] LayerMask levelCollisionLayer;

    Rigidbody2D rb;
    SpriteRenderer spriteRenderer;
    TrailRenderer trailRenderer;

    RangedAttackSO attackData;
    float curDuration;
    Vector2 dir;
    bool isReady = false;
    bool fxOnDestroy = true;


    void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        trailRenderer = GetComponent<TrailRenderer>();
    }

    void Update()
    {
        if(!isReady) return;

        curDuration += Time.deltaTime;

        if(curDuration > attackData.duration)
        {
            DestroyProjectile(transform.position, false);
        }

        rb.velocity = dir * attackData.speed;
    }

    public void InitializeAttack(Vector2 dir, RangedAttackSO attackData)
    {
        this.attackData = attackData;
        this.dir = dir;

        UpdateProjectileSprite();
        trailRenderer.Clear();
        curDuration = 0;
        spriteRenderer.color = attackData.projectileColor;

        transform.right = this.dir;

        isReady = true;
    }

    void DestroyProjectile(Vector2 dir, bool createFx)
    {
        if(createFx)
        {
            ParticleSystem particleSystem = GameManager.Instance.EffectParticle;
            
            particleSystem.transform.position = dir;
            ParticleSystem.EmissionModule em = particleSystem.emission;
            em.SetBurst(0, new ParticleSystem.Burst(0, Mathf.Ceil(attackData.size * 5)));
            ParticleSystem.MainModule mm = particleSystem.main;
            mm.startSpeedMultiplier = attackData.size * 10f;
            particleSystem.Play();
        }

        gameObject.SetActive(false);
    }

    void UpdateProjectileSprite()
    {
        transform.localScale = Vector3.one * attackData.size;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (IsLayerMatched(levelCollisionLayer.value, other.gameObject.layer))
        {
            Vector2 destroyPos = other.ClosestPoint(transform.position) - dir * 0.2f;
            DestroyProjectile(destroyPos, fxOnDestroy);
        }
        else if(IsLayerMatched(attackData.target.value, other.gameObject.layer))
        {
            // 완료 : 데미지 주기
            HealthSystem health = other.GetComponent<HealthSystem>();
            if(health != null)
            {
                bool isAttackApplied = health.ChangeHealth(-attackData.power);

                if(isAttackApplied && attackData.isOnKnockBack)
                {
                    ApplyKnockBack(other);
                }
            }

            DestroyProjectile(other.ClosestPoint(transform.position), fxOnDestroy);
        }
    }

    bool IsLayerMatched(int val, int layer)
    {
        return val == (val | 1 << layer);
    }

    void ApplyKnockBack(Collider2D col)
    {
        TopDownMovement movement = col.GetComponent<TopDownMovement>();

        if(movement != null)
        {
            movement.ApplyKnockBack(transform, attackData.knockBackPower, attackData.knockBackTime);
        }
    }    
}
