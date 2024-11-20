using UnityEngine;

public class TopDownContactEnemyController : TopDownEnemyController
{
    [SerializeField][Range(0f, 100f)] float followRange;
    [SerializeField] string targetTag = "Player";
    bool isCollidingWithTarget;

    [SerializeField] SpriteRenderer characterRenderer;
    HealthSystem healthSystem;
    HealthSystem collidingTargetHealthSystem;
    TopDownMovement collidingMovement;


    protected override void Start()
    {
        base.Start();

        healthSystem = GetComponent<HealthSystem>();
        healthSystem.OnDamage += OnDamage;
    }

    void OnDamage()
    {
        followRange = 100f;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if(isCollidingWithTarget)
        {
            ApplyHealthChange();
        }

        Vector2 dir = Vector2.zero;
        if(DistanceToTarget() < followRange)
        {
            dir = DirectionToTarget();
        }

        CallMoveEvent(dir);
        Rotate(dir);
    }

    void Rotate(Vector2 dir)
    {
        float rotZ = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        characterRenderer.flipX = Mathf.Abs(rotZ) > 90f;
    }

    void ApplyHealthChange()
    {
        AttackSO attackSO = stats.CurrentStat.attackSO;
        bool isAttackable = collidingTargetHealthSystem.ChangeHealth(-attackSO.power);

        if(isAttackable && attackSO.isOnKnockBack && collidingMovement != null)
        {
            collidingMovement.ApplyKnockBack(transform, attackSO.knockBackPower, attackSO.knockBackTime);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        GameObject receiver = other.gameObject;
        
        if(!receiver.CompareTag(targetTag))
        {
            return;
        }

        collidingTargetHealthSystem = receiver.GetComponent<HealthSystem>();
        if(collidingTargetHealthSystem != null)
        {
            isCollidingWithTarget = true;
        }

        collidingMovement = receiver.GetComponent<TopDownMovement>();
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(!other.CompareTag(targetTag)) return;

        isCollidingWithTarget = false;
    }
}