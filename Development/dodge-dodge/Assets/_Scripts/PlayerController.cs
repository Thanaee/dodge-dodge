using UnityEngine;
using UnityEngine.InputSystem; 
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    private float elapsedTime = 0f;
    private float score = 0f;
    [SerializeField] private float scoreMultiplier = 10f;
     
    [SerializeField] private float thrustForce = 1f;
    Rigidbody2D rb;

    [SerializeField] private UIDocument uiDocument;
    [SerializeField] private string mainMenuSceneName = "MainMenu";

    // UI Elemente
    private Label scoreText;
    private Label highscoreText; 
    private Button restartButton;
    private Button hudPauseButton;
    private Button hudMenuButton;
    private VisualElement pauseOverlay;
    private Button resumeButton;
    private Button pauseMenuButton;

    [SerializeField] private GameObject explosionEffect;
    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] private GameObject boosterFlame;

    private bool isPaused = false;
    private bool isGameOver = false;

    void Start()
    {
        // Zeit immer zuruecksetzen, falls vorher pausiert war
        Time.timeScale = 1f;

        rb = GetComponent<Rigidbody2D>();

        if (uiDocument != null && uiDocument.rootVisualElement != null)
        {
            var root = uiDocument.rootVisualElement;

            scoreText = root.Q<Label>("ScoreLabel");
            highscoreText = root.Q<Label>("HighscoreLabel");
            restartButton = root.Q<Button>("RestartButton");
            hudPauseButton = root.Q<Button>("HudPauseButton");
            hudMenuButton = root.Q<Button>("HudMenuButton");
            pauseOverlay = root.Q<VisualElement>("PauseOverlay");
            resumeButton = root.Q<Button>("ResumeButton");
            pauseMenuButton = root.Q<Button>("PauseMenuButton");

            if (highscoreText != null)
            {
                highscoreText.style.display = DisplayStyle.None;
            }
            
            if (restartButton != null)
            {
                restartButton.style.display = DisplayStyle.None;
                restartButton.clicked += ReloadScene; 
            }

            if (hudPauseButton != null) hudPauseButton.clicked += TogglePause;
            if (resumeButton != null) resumeButton.clicked += TogglePause;
            if (hudMenuButton != null) hudMenuButton.clicked += LoadMainMenu;
            if (pauseMenuButton != null) pauseMenuButton.clicked += LoadMainMenu;
        }
    }

    void Update()
    {
        
        if (isGameOver) return;

        //key usages esc and space
        if (Keyboard.current != null)
        {
            if (Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                TogglePause();
            }
            else if (Keyboard.current.escapeKey.wasPressedThisFrame)
            {
                if (isPaused) LoadMainMenu();
                else TogglePause();
            }
        }

        if (isPaused) return;

        elapsedTime += Time.deltaTime;
        score = Mathf.FloorToInt(elapsedTime * scoreMultiplier);
        
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }

        
        if (Mouse.current != null && Mouse.current.leftButton.isPressed)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.value);
            Vector2 direction = (mousePos - transform.position).normalized;

            transform.up = direction;
            rb.AddForce(direction * thrustForce);

            if (rb.linearVelocity.magnitude > maxSpeed)
            {
                rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
            }
        }

        if (Mouse.current != null)
        {
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                if (boosterFlame != null) boosterFlame.SetActive(true);
            }
            else if (Mouse.current.leftButton.wasReleasedThisFrame)
            {
                if (boosterFlame != null) boosterFlame.SetActive(false);
            }
        }
    }

    
    public void TogglePause()
    {
        if (isGameOver) return;

        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0f : 1f;

        if (pauseOverlay != null)
        {
            pauseOverlay.style.display = isPaused ? DisplayStyle.Flex : DisplayStyle.None;
        }
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuSceneName);
    }

    
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (isGameOver) return;
        isGameOver = true;

        int finalScore = Mathf.FloorToInt(score);
        int bestScore = PlayerPrefs.GetInt("Highscore", 0);
        if (finalScore > bestScore)
        {
            bestScore = finalScore;
            PlayerPrefs.SetInt("Highscore", bestScore);
            PlayerPrefs.Save();
        }

        if (highscoreText != null)
        {
            highscoreText.text = "Highscore: " + bestScore;
            highscoreText.style.display = DisplayStyle.Flex;
        }
        
        if (restartButton != null)
        {
            restartButton.style.display = DisplayStyle.Flex;
        }

        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, transform.rotation);
        }

        
        gameObject.SetActive(false);

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayCrash();
        }

    
    }

    void ReloadScene()
    {
        Time.timeScale = 1f; 
        PlayerPrefs.SetInt("Score", Mathf.FloorToInt(score));
        PlayerPrefs.Save();

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
