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
    private Label scoreText;
    private Label highscoreText; 
    private Button restartButton;

    [SerializeField] private GameObject explosionEffect;

     [SerializeField] private float maxSpeed = 5f;
    [SerializeField] private GameObject boosterFlame;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       rb = GetComponent<Rigidbody2D>();
       scoreText = uiDocument.rootVisualElement.Q<Label>("ScoreLabel");
       highscoreText = uiDocument.rootVisualElement.Q<Label>("HighscoreLabel");
       restartButton = uiDocument.rootVisualElement.Q<Button>("RestartButton");
        if (highscoreText != null)
        {
            highscoreText.style.display = DisplayStyle.None;
        }
        
       if (restartButton != null)
       {
        restartButton.style.display = DisplayStyle.None;
        restartButton.clicked += ReloadScene; 
       }
       
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;
        score = Mathf.FloorToInt(elapsedTime * scoreMultiplier);
        Debug.Log("Score:" + score);
        scoreText.text = "Score: " + score;
        

        if (Mouse.current.leftButton.isPressed) {
            // calculate mouse direction
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.value);
            Vector2 direction = (mousePos - transform.position).normalized ;

            //move player into direction of mouse
            transform.up = direction;
            rb.AddForce(direction * thrustForce);

            if (rb.linearVelocity.magnitude > maxSpeed)
            {
                rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
            }
        }

        if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                boosterFlame.SetActive(true);
            }
                else if (Mouse.current.leftButton.wasReleasedThisFrame)
            {
                boosterFlame.SetActive(false);
            }

        
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
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
        
        Destroy(gameObject);
        Instantiate(explosionEffect, transform.position, transform.rotation);
        restartButton.style.display = DisplayStyle.Flex;
        
    }

    void ReloadScene() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        PlayerPrefs.SetInt("Score", Mathf.FloorToInt(score));
        PlayerPrefs.Save();
        
    }


}
