using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputController : TopDownController
{
    Camera cam;

    protected override void Awake()
    {
        base.Awake();
        cam = Camera.main;
    }

    public void OnMove(InputValue val)
    {
        Vector2 moveInput = val.Get<Vector2>().normalized;
        CallMoveEvent(moveInput);
        // 실제 움직임 처리는 player movement에서 진행
    }

    public void OnLook(InputValue val)
    {
        Vector2 newAim = val.Get<Vector2>();
        Vector2 worldPos = cam.ScreenToWorldPoint(newAim);
        newAim = (worldPos - (Vector2)transform.position).normalized;

        CallLookEvent(newAim);
    }

    public void OnFire(InputValue val)
    {
        isAttacking = val.isPressed;
    }
}
