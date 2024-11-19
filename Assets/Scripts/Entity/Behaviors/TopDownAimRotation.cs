using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownAimRotation : MonoBehaviour
{
    [SerializeField] private SpriteRenderer armRenderer;
    [SerializeField] private Transform armPivot;
    
    [SerializeField] private SpriteRenderer characterRenderer;

    private TopDownController controller;
    
    void Awake()
    {
        controller = GetComponent<TopDownController>();
    }

    void Start()
    {
        // 구독
        controller.OnLookEvent += OnAim;
    }

    void OnAim(Vector2 dir)
    {
        RotateArm(dir);
    }

    void RotateArm(Vector2 dir)
    {
        // dir : 마우스의 위치를 월드 포지션으로 정규화한 값.
        // 본체에서 1만큼 떨어진, 마우스를 향한 방향.
        float rotZ = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        
        characterRenderer.flipX = Mathf.Abs(rotZ) > 90f;
        armRenderer.flipY = characterRenderer.flipX;

        armPivot.rotation = Quaternion.Euler(0, 0, rotZ);
    }
}
