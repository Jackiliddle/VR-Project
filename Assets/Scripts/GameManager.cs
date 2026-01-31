using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshProUGUI highScoreText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI gameOverText;
    public TextMeshProUGUI titleText;
    public Button restartButton;
    public GameObject titleScreen;

    [Header("Gameplay UI")]
    public TextMeshProUGUI catchText; // <-- "Catch" text (ONLY show during gameplay)

    [Header("Gameplay")]
    public PestSpawner pestSpawner;

    private int highScore;
    private int score;
    public bool isGameActive;

    [Header("Countdown")]
    public TextMeshProUGUI startText;
    public float countdownTime = 3f;

    [Header("Pause")]
    public GameObject pauseMenu;
    private bool isPaused = false;

    [Header("Instructions Panel")]
    public GameObject instructionsPanel;

    private void Start()
    {
        Time.timeScale = 1f;

        highScore = PlayerPrefs.GetInt("HighScore", 0);
        highScoreText.text = "High Score: " + highScore;

        pestSpawner.HideAllBurrows();

        // Catch text should NOT show on title screen / startup
        if (catchText != null)
            catchText.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (isGameActive && Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void UpdateScore(int scoreToAdd)
    {
        score += scoreToAdd;
        scoreText.text = "Current score: " + score;

        if (score > highScore)
        {
            highScore = score;
            highScoreText.text = "High Score: " + highScore;
            PlayerPrefs.SetInt("HighScore", highScore);
            PlayerPrefs.Save();
        }
    }

    public void StartGame(int difficulty)
    {
        Time.timeScale = 1f;
        titleScreen.SetActive(false);

        // Hide Catch during countdown
        if (catchText != null)
            catchText.gameObject.SetActive(false);

        StartCoroutine(CountdownAndStart(difficulty));
    }

    private IEnumerator CountdownAndStart(int difficulty)
    {
        startText.gameObject.SetActive(true);

        for (int i = (int)countdownTime; i > 0; i--)
        {
            startText.text = i.ToString();
            yield return new WaitForSeconds(1f);
        }

        startText.text = "GO!";
        yield return new WaitForSeconds(1f);
        startText.gameObject.SetActive(false);

        isGameActive = true;

        // Show Catch ONLY when gameplay is actually running
        if (catchText != null)
            catchText.gameObject.SetActive(true);

        score = 0;
        UpdateScore(0);

        if (pestSpawner != null)
            pestSpawner.AdjustDifficulty(difficulty);

        CropGrower.StartGrowingCrops();
        RootCropGrower.StartGrowingCrops();
    }

    public void CheckGameOver()
    {
        GameObject[] veggies = GameObject.FindGameObjectsWithTag("Veggie");
        Debug.Log($"Veggies remaining: {veggies.Length}");

        if (veggies.Length == 0)
            GameOver();
    }

    public void GameOver()
    {
        if (!isGameActive) return;

        gameOverText.gameObject.SetActive(true);
        isGameActive = false;

        // Hide Catch on game over
        if (catchText != null)
            catchText.gameObject.SetActive(false);

        if (pestSpawner != null)
            pestSpawner.StopAllCoroutines();

        restartButton.gameObject.SetActive(true);

        Debug.Log("GAME OVER! All veggies have been eaten!");
        Debug.Log("Score: " + score);
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Debug.Log("Restart Clicked!");
    }

    public void ShowInstructions()
    {
        if (instructionsPanel != null)
            instructionsPanel.SetActive(true);
    }

    public void HideInstructions()
    {
        if (instructionsPanel != null)
            instructionsPanel.SetActive(false);
    }

    public void TogglePause()
    {
        if (isPaused)
        {
            // Unpause
            Time.timeScale = 1f;
            isPaused = false;
            if (pauseMenu != null) pauseMenu.SetActive(false);

            // Show Catch again ONLY if game is active
            if (catchText != null && isGameActive)
                catchText.gameObject.SetActive(true);
        }
        else
        {
            // Pause
            Time.timeScale = 0f;
            isPaused = true;
            if (pauseMenu != null) pauseMenu.SetActive(true);

            // Hide Catch while paused
            if (catchText != null)
                catchText.gameObject.SetActive(false);
        }
    }

    //Do not delete! Fixes the game loop issue!
    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        isPaused = false;
        isGameActive = false;

        if (pauseMenu != null)
            pauseMenu.SetActive(false);

        if (pestSpawner != null)
            pestSpawner.HideAllBurrows();

        titleScreen.SetActive(true);
        gameOverText.gameObject.SetActive(false);
        restartButton.gameObject.SetActive(false);

        // Hide Catch on main menu / title screen
        if (catchText != null)
            catchText.gameObject.SetActive(false);

        Debug.Log("Returned to Main Menu");
    }
}
