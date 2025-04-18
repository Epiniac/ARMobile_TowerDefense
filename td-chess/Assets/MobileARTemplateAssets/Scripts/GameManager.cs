using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public bool loopShouldEnd;
    public GameObject winPanel;
    public GameObject gameOverPanel;

    public Text livesText;
    public Text scoreText;
    public Text moneyText;

    public Player player;

    void Start()
    {
        player = new Player();
        winPanel.SetActive(false);
        gameOverPanel.SetActive(false);
        UpdateUI();
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
        while (loopShouldEnd == false)
        {
            // Spawn ennemies
        
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

