using System;
using UnityEngine;

public class TopDownAnimationController : AnimationController
{
    private static readonly int isWalking = Animator.StringToHash("IsWalking");
    private static readonly int isHit = Animator.StringToHash("IsHit");
    private static readonly int attack = Animator.StringToHash("Attack");
    private readonly float magnitudeThreshold = 0.5f;
    HealthSystem healthSystem;

    protected override void Awake()
    {
        base.Awake();

        healthSystem = GetComponent<HealthSystem>();
    }

    void Start()
    {
        controller.OnAttackEvent += Attacking;
        controller.OnMoveEvent += Move;

        if(healthSystem != null)
        {
            healthSystem.OnDamage += Hit;
            healthSystem.OnInvincibilityEnd += InvincibilityEnd;
        }
    }

    void Move(Vector2 dir)
    {
        anim.SetBool(isWalking, dir.magnitude > magnitudeThreshold);
    }

    void Attacking(AttackSO attackSO)
    {
        anim.SetTrigger(attack);
    }

    void Hit()
    {
        anim.SetBool(isHit, true);
    }

    void InvincibilityEnd()
    {
        anim.SetBool(isHit, false);
    }
}