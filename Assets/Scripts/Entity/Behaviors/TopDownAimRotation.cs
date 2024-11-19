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
        // ����
        controller.OnLookEvent += OnAim;
    }

    void OnAim(Vector2 dir)
    {
        RotateArm(dir);
    }

    void RotateArm(Vector2 dir)
    {
        // dir : ���콺�� ��ġ�� ���� ���������� ����ȭ�� ��.
        // ��ü���� 1��ŭ ������, ���콺�� ���� ����.
        float rotZ = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        
        characterRenderer.flipX = Mathf.Abs(rotZ) > 90f;
        armRenderer.flipY = characterRenderer.flipX;

        armPivot.rotation = Quaternion.Euler(0, 0, rotZ);
    }
}
