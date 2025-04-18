using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject winPanel;
    public GameObject gameOverPanel;

    public Text livesText;
    public Text scoreText;
    public Text moneyText;

    public Player player;

    private void Awake()
    {
        Instance = this;
    }

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
}

[System.Serializable]
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