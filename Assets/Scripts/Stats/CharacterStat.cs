using UnityEngine;

public enum StatsChangeType
{
    Add, // 0
    Multiple, // 1
    Override  // 2
}

// 데이터 폴더처럼 사용 가능
[System.Serializable]
public class CharacterStat
{
    public StatsChangeType statsChangeType;

    [Range(0, 100)] public int maxHealth;
    [Range(0f, 20f)] public float speed;
    
    public AttackSO attackSO;
}