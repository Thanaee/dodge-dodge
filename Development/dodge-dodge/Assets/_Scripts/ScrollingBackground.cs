using UnityEngine;

public class ScrollingBackground : MonoBehaviour
{
    [SerializeField] private float speedX = 0.05f;
    [SerializeField] private float speedY = 0f;

    private Renderer meshRenderer;

    void Start()
    {
        meshRenderer = GetComponent<Renderer>();
    }

    void Update()
    {
        meshRenderer.material.mainTextureOffset = new Vector2(Time.time * speedX, Time.time * speedY);
    }
}
