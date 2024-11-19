using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class TopDownShooting : MonoBehaviour
{
    TopDownController controller;
    [SerializeField] Transform tfFirePoint;
    Vector2 aimDir = Vector2.right;
    // ObjectPool pool;

    [SerializeField] AudioClip ShootingClip;

    void Awake()
    {
        controller = GetComponent<TopDownController>();
    }

    void Start()
    {
        controller.OnAttackEvent += OnShoot;
        controller.OnLookEvent += OnAim;
    }

    void OnAim(Vector2 dir)
    {
        aimDir = dir;
    }

    void OnShoot(AttackSO attackSO)
    {
        RangedAttackSO rangedAttackSO = attackSO as RangedAttackSO;

        if(rangedAttackSO == null)
            return;

        float projectileAngleSpace = rangedAttackSO.multipleProjectilesAngle;
        int numOfProjectilePerShot = rangedAttackSO.numberOfProjectilesPerShot;

        //
        float minAngle = -(numOfProjectilePerShot / 2f) * projectileAngleSpace + 0.5f * rangedAttackSO.multipleProjectilesAngle;
        for (int i = 0; i < numOfProjectilePerShot; i++)
        {
            float angle = minAngle + i * projectileAngleSpace;
            float randomSpread = Random.Range(-rangedAttackSO.spread, rangedAttackSO.spread);
            angle += randomSpread;

            CreateProjectile(rangedAttackSO, angle);
        }
    }

    void CreateProjectile(RangedAttackSO rangedAttackSO, float angle)
    {
        GameObject go = GameManager.Instance.ObjectPool.SpawnFromPool(rangedAttackSO.bulletNameTag);
        go.transform.position = tfFirePoint.position;
        
        ProjectileController attackController = go.GetComponent<ProjectileController>();
        attackController.InitializeAttack(RotateVector2(aimDir, angle), rangedAttackSO);
        
        if(ShootingClip) SoundManager.PlayClip(ShootingClip);
    }

    private static Vector2 RotateVector2(Vector2 v, float angle)
    {
        return Quaternion.Euler(0f, 0f, angle) * v;
    }
}
