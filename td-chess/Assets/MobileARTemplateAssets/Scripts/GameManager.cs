using UnityEngine.Jobs;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Unity.Jobs;
using Unity.Collections;
using Unity.Burst;
using Unity.Mathematics;
using Unity.Collections.LowLevel.Unsafe;
using TMPro;

public class GameManager : MonoBehaviour
{
    public GameObject winPanel;
    public GameObject gameOverPanel;

    public TMP_Text livesText;
    public TMP_Text scoreText;
    public TMP_Text moneyText;

    public Player player;

    private static Queue<int> EnemyIDsToSummon;
    private static Queue<Enemy> EnemiesToRemove;

    public static Vector3[] NodePositions;
    public Transform NodeParent;
    public bool loopShouldEnd;

    public void Start()
    {
        EnemyIDsToSummon = new Queue<int>();
        EnemiesToRemove = new Queue<Enemy>();
        player = new Player();
        winPanel.SetActive(false);
        gameOverPanel.SetActive(false);
        UpdateUI();

        EntitySummoner.Init();

        NodePositions = new Vector3[NodeParent.childCount];
        for (int i = 0; i < NodePositions.Length; i++)
        {
            NodePositions[i] = NodeParent.GetChild(i).position;
        }
        
        StartCoroutine(GameLoop());
        StartCoroutine(SummonEnemy(1f));
    }

    public void AddScore(int amount)
    {
        player.score += amount;
        UpdateUI();
    }

    public void SpendMoney(int amount)
    {
        if (player.money >= amount)
        {
            player.money -= amount;
            UpdateUI();
        }
        else
        {
            Debug.Log("Not enough money!");
        }
    }

    public void LoseLife()
    {
        player.lives--;
        UpdateUI();

        if (player.lives <= 0)
        {
            GameOver();
        }
    }

    public void WinGame()
    {
        winPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void GameOver()
    {
        gameOverPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void UpdateUI()
    {
        livesText.text = "Lives: " + player.lives;
        scoreText.text = "Score: " + player.score;
        moneyText.text = "Money: " + player.money;
    }
    

    IEnumerator GameLoop()
    {
        
        loopShouldEnd = false;
        while (!loopShouldEnd)
        {
            // Spawn enemies from the queue
            while (EnemyIDsToSummon.Count > 0)
            {
                EntitySummoner.SummonEnemy(EnemyIDsToSummon.Dequeue());
            }

            // Move enemies
            if (EntitySummoner.EnemiesInGame.Count > 0)
            {
                NativeArray<Vector3> Nodes = new NativeArray<Vector3>(NodePositions, Allocator.TempJob);
                NativeArray<int> NodeIndexes = new NativeArray<int>(EntitySummoner.EnemiesInGame.Count, Allocator.TempJob);
                NativeArray<float> Speeds = new NativeArray<float>(EntitySummoner.EnemiesInGame.Count, Allocator.TempJob);
                TransformAccessArray EnemyAccess = new TransformAccessArray(EntitySummoner.EnemiesInGameTransform.ToArray());

                for (int i = 0; i < EntitySummoner.EnemiesInGame.Count; i++)
                {
                    NodeIndexes[i] = EntitySummoner.EnemiesInGame[i].NodeIndex;
                    Speeds[i] = EntitySummoner.EnemiesInGame[i].Speed;
                }

                EnemyMoveJob moveJob = new EnemyMoveJob
                {
                    Nodes = Nodes,
                    NodeIndexes = NodeIndexes,
                    Speeds = Speeds,
                    DeltaTime = Time.deltaTime
                };

                JobHandle moveJobHandle = moveJob.Schedule(EnemyAccess);
                moveJobHandle.Complete();

                for (int i = 0; i < EntitySummoner.EnemiesInGame.Count; i++)
                {
                    EntitySummoner.EnemiesInGame[i].NodeIndex = NodeIndexes[i];

                    if (EntitySummoner.EnemiesInGame[i].NodeIndex >= NodePositions.Length)
                    {
                        EnqueueEnemyToRemove(EntitySummoner.EnemiesInGame[i]);
                    }
                }

                Nodes.Dispose();
                NodeIndexes.Dispose();
                Speeds.Dispose();
                EnemyAccess.Dispose();
            }

            // Remove enemies that reached the end
            if (EnemiesToRemove.Count > 0)
            {
                for (int i = 0; i < EnemiesToRemove.Count; i++)
                {
                    EntitySummoner.RemoveEnemy(EnemiesToRemove.Dequeue());
                    LoseLife();
                }
            }

            yield return null;
            }
    }

    public static void EnqueueEnemyIDToSummon(int enemyID)
    {
        EnemyIDsToSummon.Enqueue(enemyID);
    }

    public static void EnqueueEnemyToRemove(Enemy EnemyToRemove)
    {
        EnemiesToRemove.Enqueue(EnemyToRemove);
    }

    IEnumerator SummonEnemy(float delay)
    {
        int EnemyID1 = 1;
        int EnemyID2 = 2;

        for (int i = 0; i < 10; i++)
        {
            EnqueueEnemyIDToSummon(EnemyID1);
            yield return new WaitForSeconds(delay);
        }
        for (int i = 0; i < 10; i++)
        {
            EnqueueEnemyIDToSummon(EnemyID2);
            yield return new WaitForSeconds(delay + 0.5f);
        }
    }
}

#if UNITY_BURST
[BurstCompile]
#endif
public struct EnemyMoveJob : IJobParallelForTransform
{
    [NativeDisableParallelForRestriction]
    public NativeArray<Vector3> Nodes;
    public NativeArray<int> NodeIndexes;
    public NativeArray<float> Speeds;
    public float DeltaTime;

    public void Execute(int index, TransformAccess transform)
    {
        if(NodeIndexes[index] >= Nodes.Length)
        {
            return;
        }
        Vector3 targetPosition = Nodes[NodeIndexes[index]];
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, Speeds[index] * DeltaTime);
        if(transform.position == targetPosition)
        {
            NodeIndexes[index]++;

        }
    }
}
public class Player
{
    public int lives { get; set; }
    public int score { get; set; }
    public int money { get; set; }

    public Player()
    {
        lives = 3;
        money = 100;
        score = 0;
    }
}

