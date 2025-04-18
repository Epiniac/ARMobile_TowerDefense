using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public bool loopShouldEnd;
    public GameObject winPanel;
    public GameObject gameOverPanel;

    public Text livesText;
    public Text scoreText;
    public Text moneyText;

    public Player player;

    private static Queue<int> EnemyIDsToSummon;

    public void Start()
    {
        EnemyIDsToSummon = new Queue<int>();
        player = new Player();
        winPanel.SetActive(false);
        gameOverPanel.SetActive(false);
        UpdateUI();

        EntitySummoner.Init();
        
        StartCoroutine(GameLoop());
        // Test summoning enemies
        InvokeRepeating("SummonTest", 0f, 1f);
        InvokeRepeating("RemoveTest", 0f, 0.5f);
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

    void RemoveTest()
    {
        if (EntitySummoner.EnemiesInGame.Count > 0)
        {
            EntitySummoner.RemoveEnemy(EntitySummoner.EnemiesInGame[Random.Range(0, EntitySummoner.EnemiesInGame.Count)]);
        }
    }
    void SummonTest()
    {
        EnqueueEnemy(0);
    }
    
    IEnumerator GameLoop()
    {
        loopShouldEnd = false;
        while (loopShouldEnd == false)
        {
            EnqueueEnemy(0); // Test summoning enemies
            // Spawn ennemies
            if (EnemyIDsToSummon.Count > 0)
            {
                for (int i = 0; i < EnemyIDsToSummon.Count; i++)
                {
                    EntitySummoner.SummonEnemy(EnemyIDsToSummon.Dequeue());
                }
            }
        
            // Spawn towers
        
            // Move ennemies
        
            // Tick towers
        
            // Apply effects
        
            // Damage ennemies
        
            // Remove ennemies
        
            // Remove towers

            yield return null;
        }
    }

    public static void EnqueueEnemy(int enemyID)
    {
        EnemyIDsToSummon.Enqueue(enemyID);
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

