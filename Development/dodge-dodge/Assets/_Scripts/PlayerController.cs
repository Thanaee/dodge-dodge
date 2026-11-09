using UnityEngine;
using UnityEngine.InputSystem; 

public class PlayerController : MonoBehaviour
{
     
     private float elapsedTime = 0f;
     private float score = 0f;
     [SerializeField] private float scoreMultiplier = 10f;
     
     
     [SerializeField] private float thrustForce = 1f;
     Rigidbody2D rb;

     [SerializeField] private float maxSpeed = 5f;
     [SerializeField] private GameObject boosterFlame;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;
        score = Mathf.FloorToInt(elapsedTime * scoreMultiplier); 
        

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
        Destroy(gameObject);
    }
}
