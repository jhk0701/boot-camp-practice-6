using UnityEngine;

public class TopDownRangedEnemyController : TopDownEnemyController
{
    [SerializeField] [Range(0f, 100f)] float followRange = 15f;
    [SerializeField] [Range(0f, 100f)] float shootRange = 10f;

    int layerMaskTarget;

    protected override void Start()
    {
        base.Start();
        layerMaskTarget = stats.CurrentStat.attackSO.target;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        float distToTarget = DistanceToTarget();
        Vector2 dirToTarget = DirectionToTarget();

        UpdateEnemyState(distToTarget, dirToTarget);
    }

    void UpdateEnemyState(float dist, Vector2 dir)
    {
        isAttacking = false;

        if(dist < followRange)
        {
            CheckIfNear(dist, dir);
        }
    }

    void CheckIfNear(float dist, Vector2 dir)
    {
        if(dist <= shootRange)
        {
            TryShootAtTarget(dir);
        }
        else
        {
            CallMoveEvent(dir);   
        }
    }

    void TryShootAtTarget(Vector2 dir)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, shootRange, layerMaskTarget);

        if(hit.collider != null)
        {
            PerformAttackAction(dir);
        }
        else
        {
            CallMoveEvent(dir);
        }
    }
    
    void PerformAttackAction(Vector2 dir)
    {
        CallLookEvent(dir);
        CallMoveEvent(Vector2.zero);
        isAttacking = true;
    }

}