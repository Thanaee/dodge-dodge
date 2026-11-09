using UnityEngine;

public class Obstacle : MonoBehaviour
{
   [SerializeField] private float minSize = 0.1f;
   [SerializeField] private float maxSize = 4.0f;

   [SerializeField] private float minSpeed = 50f;
   [SerializeField] private float maxSpeed = 150f;
   [SerializeField] private float maxSpinSpeed = 10f;


    Rigidbody2D rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        float randomSize = Random.Range(minSize,maxSize);
        transform.localScale = new Vector3(randomSize,randomSize,1);

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
        
    }
}
