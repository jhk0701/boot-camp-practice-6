using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharacterStatsHandler : MonoBehaviour 
{
    // 기본 스탯과 추가 스탯들을 계산해서 최종 스탯을 계산하는 로직
    [SerializeField] CharacterStat baseStat;
    public CharacterStat CurrentStat { get; private set; } = new CharacterStat();

    public List<CharacterStat> statModifiers = new List<CharacterStat>();

    readonly float MinAttackDelay = 0.03f;
    readonly float MinAttackPower = 0.5f;
    readonly float MinAttackSize = 0.4f;
    readonly float MinAttackSpeed = 0.1f;

    readonly float MinSpeed = 0.8f;
    readonly int MinMaxHealth = 5;


    void Awake()
    {
        UpdateCharacterStat();
        InitializeStat();
    }

    void InitializeStat()
    {
        // 기본 스텟 초기화
        if(baseStat.attackSO != null)
        {
            baseStat.attackSO = Instantiate(baseStat.attackSO);
            CurrentStat.attackSO = Instantiate(baseStat.attackSO);
        }
    }

    void UpdateCharacterStat()
    {
        ApplyStatModifier(baseStat);

        foreach(CharacterStat stat in statModifiers.OrderBy(n=>n.statsChangeType))
        {
            ApplyStatModifier(stat);
        }

        return;
        AttackSO attackSO = null;
        if(baseStat.attackSO != null)
        {
            attackSO = Instantiate(baseStat.attackSO);
        }

        CurrentStat = new CharacterStat{ attackSO = attackSO };
        
        // TODO : 지금은 기본 능력치만 적용됨, 나중에 능력치 강화 기능이 적용됨.
        CurrentStat.statsChangeType = baseStat.statsChangeType;
        CurrentStat.maxHealth = baseStat.maxHealth;
        CurrentStat.speed = baseStat.speed;
    }

    public void AddStatModifier(CharacterStat stat)
    {
        statModifiers.Add(stat);
        UpdateCharacterStat();
    }

    public void RemoveStatModifier(CharacterStat stat)
    {
        statModifiers.Remove(stat);
        UpdateCharacterStat();
    }

    private void ApplyStatModifier(CharacterStat modifier)
    {
        // Func<float, float, float> operation = modifier.statsChangeType switch
        // {
        //     StatsChangeType.Add => (current, change) => current + change,
        //     StatsChangeType.Multiple => (current, change) => current * change,
        //     StatsChangeType.Override => (current, change) => change,
        //     _ => (current, change) => change // default 케이스, Override 부분을 이렇게도 쓸 수 있음
        // };

        UpdateBasicStats(GetOperation(modifier), modifier);
        UpdateAttackStats(GetOperation(modifier), modifier);

        if(CurrentStat.attackSO is RangedAttackSO currentRanged && modifier.attackSO is RangedAttackSO newRanged)
        {
            UpdateRangedAttackStats(GetOperation(modifier), currentRanged, newRanged);
        }
    }

    Func<float, float, float> GetOperation(CharacterStat stat)
    {
        return stat.statsChangeType switch
        {
            StatsChangeType.Add => (current, change) => current + change,
            StatsChangeType.Multiple => (current, change) => current * change,
            StatsChangeType.Override => (current, change) => change,
            _ => (current, change) => change // default 케이스, Override 부분을 이렇게도 쓸 수 있음
        };
    }

    private void UpdateBasicStats(Func<float, float, float> operation, CharacterStat modifier)
    {
        // Max를 통한 최솟갑 적용
        CurrentStat.maxHealth = Mathf.Max((int)operation(CurrentStat.maxHealth, modifier.maxHealth), MinMaxHealth);
        
        CurrentStat.speed = Mathf.Max((int)operation(CurrentStat.speed, modifier.speed), MinSpeed);
    }

    private void UpdateAttackStats(Func<float, float, float> operation, CharacterStat modifier)
    {
        if(CurrentStat.attackSO == null || modifier.attackSO == null) return;

        var currentAttack = CurrentStat.attackSO;
        var newAttack = modifier.attackSO;

        currentAttack.delay = Mathf.Max(operation(currentAttack.delay, newAttack.delay), MinAttackDelay);
        currentAttack.power = Mathf.Max(operation(currentAttack.power, newAttack.power), MinAttackPower);
        currentAttack.size = Mathf.Max(operation(currentAttack.size, newAttack.size), MinAttackSize);
        currentAttack.speed = Mathf.Max(operation(currentAttack.speed, newAttack.speed), MinAttackSpeed);
    }

    private void UpdateRangedAttackStats(Func<float, float, float> operation, RangedAttackSO currentRanged, RangedAttackSO newRanged)
    {
        currentRanged.multipleProjectilesAngle = operation(currentRanged.multipleProjectilesAngle, newRanged.multipleProjectilesAngle);
        currentRanged.spread = operation(currentRanged.spread, newRanged.spread);
        currentRanged.duration = operation(currentRanged.duration, newRanged.duration);
        currentRanged.projectileColor = UpdateColor(operation, currentRanged.projectileColor, newRanged.projectileColor);
    }

    private Color UpdateColor(Func<float, float, float> operation, Color current, Color modifier)
    {
        return new Color
        (
            operation(current.r, modifier.r),
            operation(current.g, modifier.g),
            operation(current.b, modifier.b),
            operation(current.a, modifier.a)
        );
    }
}