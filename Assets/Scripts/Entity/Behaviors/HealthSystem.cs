using System;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] float healthChangeDelay = 0.5f;
    [SerializeField] AudioClip damageClip;

    CharacterStatsHandler statHandler;
    float timeSinceLastChange = float.MaxValue;
    bool isAttacked = false;

    public event Action OnDamage;
    public event Action OnHeal;
    public event Action OnDeath;
    public event Action OnInvincibilityEnd;
    
    [field:SerializeField] public float CurrentHealth { get; private set; }

    // get 변수처럼 사용할 수 있도록 설정
    public float MaxHealth => statHandler.CurrentStat.maxHealth;

    void Awake()
    {
        statHandler = GetComponent<CharacterStatsHandler>();
    }

    void Start()
    {
        CurrentHealth = MaxHealth;
    }

    void Update()
    {
        if(isAttacked && timeSinceLastChange < healthChangeDelay)
        {
            timeSinceLastChange += Time.deltaTime;

            if(timeSinceLastChange >= healthChangeDelay)
            {
                OnInvincibilityEnd?.Invoke();
                isAttacked = false;
            }
        }
    }

    public bool ChangeHealth(float change)
    {
        if(timeSinceLastChange < healthChangeDelay)
        {
            // 공격을 하지 않고 끝나는 상황
            return false;
        }

        timeSinceLastChange = 0f;
        CurrentHealth += change;
        CurrentHealth = Mathf.Clamp(CurrentHealth, 0, MaxHealth);

        if(CurrentHealth <= 0f)
        {
            CallDeath();
            return false;
        }

        if(change >= 0)
        {
            OnHeal?.Invoke();
        }
        else
        {
            OnDamage?.Invoke();
            isAttacked = true;

            if(damageClip) SoundManager.PlayClip(damageClip);
        }
        return true;
    }


    void CallDeath()
    {
        OnDeath?.Invoke();
    }
}   