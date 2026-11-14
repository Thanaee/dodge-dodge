using UnityEngine;

public class Obstacle : MonoBehaviour
{
   [SerializeField] private float minSize = 0.5f;
   [SerializeField] private float maxSize = 2.5f;
   [SerializeField] private GameObject bounceEffectPrefab;

   [SerializeField] private float minSpeed = 50f;
   [SerializeField] private float maxSpeed = 150f;
   [SerializeField] private float maxSpinSpeed = 10f;
   [SerializeField] private float safeRadiusFromPlayer = 4f;


    Rigidbody2D rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            Vector2 spawnPos = transform.position;
            int attempts = 0;
            
            while (Vector2.Distance(spawnPos, player.transform.position) < safeRadiusFromPlayer && attempts <25)
            {
                spawnPos = new Vector2(Random.Range(-8f, 8f), Random.Range(-4.5f, 4.5f));
                attempts++;
            }
            transform.position = spawnPos;
        }

        //set size 
        float randomSize = Random.Range(minSize,maxSize);
        transform.localScale = new Vector3(randomSize,randomSize,1);

        //set speed
        float randomSpeed = Random.Range(minSpeed,maxSpeed) / randomSize;
        Vector2 randomDirection = Random.insideUnitCircle;
        rb = GetComponent <Rigidbody2D>();
        rb.AddForce(randomDirection * randomSpeed);

        float randomTorque = Random.Range(-maxSpinSpeed, maxSpinSpeed);
        rb.AddTorque(randomTorque);
    }

    // Update is called once per frame
    void Update()
    {
        
       float maxVelocity = 12f;
    
            //increasing difficulty and pace overtime
        if (rb != null && rb.linearVelocity.magnitude > maxSpeed)
        {
            rb.linearVelocity *= 1f + (0.02f * Time.fixedDeltaTime);

            //restricts the speed at some point for maxVelocity
            if (rb.linearVelocity.magnitude > maxVelocity)
            {
                rb.linearVelocity = rb.linearVelocity.normalized * maxVelocity;
            }

        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
         Vector2 contactPoint = collision.GetContact(0).point;
            GameObject bounceEffect = Instantiate (bounceEffectPrefab, contactPoint, Quaternion.identity);

            Destroy(bounceEffect, 1f);
    }
   
}
