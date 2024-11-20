
using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using TMPro;

using Random = UnityEngine.Random;

public enum UpgradeOption
{
    MaxHealth,
    AttackPower,
    Speed,
    Knockback,
    AttackDelay,
    NumberOfProjectile,
    COUNT // 실제 쓰이는 enum이 아니라 몇 개가 들어 있는지에 대한 값
}


public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    [SerializeField] string playerTag;
    [SerializeField] CharacterStat defaultStat;
    [SerializeField] CharacterStat rangedStat;

    public Transform Player { get; private set; }

    public ObjectPool ObjectPool { get; private set; }

    public ParticleSystem EffectParticle { get; set; }


    HealthSystem playerHealthSystem;

    [SerializeField] TextMeshProUGUI waveText;
    [SerializeField] Slider hpGaugeSlider;
    [SerializeField] GameObject gameOverUI;

    [Header("Wave")]
    [SerializeField] int currentWaveIndex = 0;
    int currentSpawnCount = 0;
    int waveSpawnCount = 0;
    int waveSpawnPosCount = 0;
    
    public float spawnInterval = 0.5f;
    public List<GameObject> enemyPrefabs = new List<GameObject>();

    [SerializeField] Transform spawnPositionsRoot;
    List<Transform> spawnPositions = new List<Transform>();

    [SerializeField] List<GameObject> Rewards = new List<GameObject>();

    private void Awake()
    {
        if(Instance != null)
            Destroy(gameObject);

        Instance = this;

        Player = GameObject.FindGameObjectWithTag(playerTag).transform;
        ObjectPool = GetComponent<ObjectPool>();
        EffectParticle = GameObject.FindGameObjectWithTag("Particle").GetComponent<ParticleSystem>();

        
        playerHealthSystem = Player.GetComponent<HealthSystem>();
        playerHealthSystem.OnDamage += UpdateHealthUI;
        playerHealthSystem.OnHeal += UpdateHealthUI;
        playerHealthSystem.OnDeath += GameOver;

        UpgradeStatInit();

        for (int i = 0; i < spawnPositionsRoot.childCount; i++)
        {
            spawnPositions.Add(spawnPositionsRoot.GetChild(i));
        }
    }

    private void UpgradeStatInit()
    {
        defaultStat.statsChangeType = StatsChangeType.Add;
        defaultStat.attackSO = Instantiate(defaultStat.attackSO);

        rangedStat.statsChangeType = StatsChangeType.Add;
        rangedStat.attackSO = Instantiate(rangedStat.attackSO);
    }

    private void Start() 
    {
        StartCoroutine(StartNextWave());    
    }

    IEnumerator StartNextWave()
    {
        while(true)
        {
            if (currentSpawnCount == 0)
            {
                UpdateWaveUI();

                yield return new WaitForSeconds(2f);

                ProcessWaveConditions();

                yield return StartCoroutine(SpawnEnemiesInWave());

                currentWaveIndex++;
            }

            yield return null;
        }
    }

    private IEnumerator SpawnEnemiesInWave()
    {
        for (int i = 0; i < waveSpawnCount; i++)
        {
            int posIndex = Random.Range(0, spawnPositions.Count);
            for (int j = 0; j < waveSpawnCount; j++)
            {
                SpawnEnemiesAtPosition(posIndex);
                yield return new WaitForSeconds(spawnInterval);
            }
        }
    }

    private void SpawnEnemiesAtPosition(int posIndex)
    {
        int prefabId = Random.Range(0, enemyPrefabs.Count);
        GameObject enemy = Instantiate(enemyPrefabs[prefabId], spawnPositions[posIndex].position, Quaternion.identity);
        enemy.GetComponent<CharacterStatsHandler>().AddStatModifier(defaultStat);
        enemy.GetComponent<CharacterStatsHandler>().AddStatModifier(rangedStat);
        enemy.GetComponent<HealthSystem>().OnDeath += OnEnemyDeath;
        currentSpawnCount++;
    }

    private void OnEnemyDeath()
    {
        currentSpawnCount--;
    }

    private void ProcessWaveConditions()
    {
        if (currentWaveIndex % 20 == 9)
        {
            RandomDebuff();
        }

        if(currentWaveIndex % 20 == 0)
        {
            RandomUpgrade();
        }

        if(currentWaveIndex % 10 == 0)
        {
            IncreaseSpawnPositions();
        }

        if (currentWaveIndex % 5 == 0)
        {
            CreateReward();
        }

        if(currentSpawnCount % 3 == 0)
        {
            IncreaseWaveSpawnCount();
        }
        
    }


    private void IncreaseWaveSpawnCount()
    {
        // waveSpawnCount++;
        waveSpawnCount = 1;
    }

    private void CreateReward()
    {
        Debug.Log("Create Reward 호출");
        int selectedRewardIndex = Random.Range(0, Rewards.Count);
        int randomPositionIndex = Random.Range(0, spawnPositions.Count);

        GameObject obj = Rewards[selectedRewardIndex];
        Instantiate(obj, spawnPositions[randomPositionIndex].position, Quaternion.identity);
        
    }

    private void IncreaseSpawnPositions()
    {
        // 예외처리
        waveSpawnPosCount = waveSpawnPosCount + 1 > spawnPositions.Count ? waveSpawnCount : waveSpawnPosCount + 1;

        // 초기화
        waveSpawnCount = 0;
    }

    private void RandomUpgrade()
    {
        UpgradeOption option = (UpgradeOption)Random.Range(0, (int)UpgradeOption.COUNT);
        switch (option)
        {
            case UpgradeOption.MaxHealth :
                defaultStat.maxHealth += 2;
                break;
                
            case UpgradeOption.AttackPower :
                defaultStat.attackSO.power += 1;
                break;
                
            case UpgradeOption.Speed :
                defaultStat.speed += 0.1f;
                break;
            
            case UpgradeOption.Knockback :
                defaultStat.attackSO.isOnKnockBack = true;
                defaultStat.attackSO.knockBackPower += 1;
                defaultStat.attackSO.knockBackTime = 0.1f;
                break;

            case UpgradeOption.AttackDelay :
                defaultStat.attackSO.delay -= 0.05f;
                break;
            
            case UpgradeOption.NumberOfProjectile:
                RangedAttackSO rangedAttack = rangedStat.attackSO as RangedAttackSO;
                if (rangedAttack != null) rangedAttack.numberOfProjectilesPerShot += 1;
                break;
            
            default :
                break;
        }
    }
    
    private void RandomDebuff()
    {
        // 체력을 0%~50%를 감소시키기
        float amount = Random.Range(0.00f, 0.51f);
        float damage = playerHealthSystem.CurrentHealth * (1f - amount);
        
        Debug.Log($"디버프 데미지 : {damage}");

        playerHealthSystem.ChangeHealth(-damage);
    }

    private void UpdateWaveUI()
    {
        // waveText.text = (currentWaveIndex + 1).ToString();
        waveText.text = $"{currentWaveIndex + 1}"; // 보간 문자열
    }


    void UpdateHealthUI()
    {
        hpGaugeSlider.value = playerHealthSystem.CurrentHealth / playerHealthSystem.MaxHealth;
    }

    void GameOver()
    {
        // UI 켜주기 
        gameOverUI.SetActive(true);
        // 게임 멈추기
    }


    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
    }
}
