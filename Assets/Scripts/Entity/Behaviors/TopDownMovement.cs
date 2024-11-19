using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class TopDownMovement : MonoBehaviour
{
    private TopDownController controller;
    private Rigidbody2D rb;
    private CharacterStatsHandler characterStatsHandler;
    
    private Vector2 movementDir = Vector2.zero;
    private Vector2 knockBack = Vector2.zero;
    float knockBackDuration = 0f;

    void Awake()
    {
        // 주로 내 컴포넌트 안에서 끝나는 거
        // controller는 movement와 같은 곳에 있음
        controller = GetComponent<TopDownController>();
        rb = GetComponent<Rigidbody2D>();
        characterStatsHandler = GetComponent<CharacterStatsHandler>();
    }

    void Start()
    {
        controller.OnMoveEvent += Move;
    }

    private void Move(Vector2 dir)
    {
        movementDir = dir;
    }


    void FixedUpdate()
    {
        // 실제로 물리 움직임이 처리되는 곳
        ApplyMovement(movementDir);

        // Fixed Update이므로 Time.deltaTime이 아님.
        // Time.fixedDeltaTime 사용
        if(knockBackDuration > 0.0f)
            knockBackDuration -= Time.fixedDeltaTime;
    }


    private void ApplyMovement(Vector2 dir)
    {
        // 캐릭터 스탯의 스피드를 적용
        dir *= characterStatsHandler.CurrentStat.speed;

        if(knockBackDuration > 0.0f)
        {
            dir += knockBack;
        }

        rb.velocity = dir;
    }

    public void ApplyKnockBack(Transform other, float power, float duration)
    {
        knockBackDuration = duration;
        knockBack = -(other.position - transform.position).normalized * power;
    }
}
