using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;


public class DynamicPostProcessing : MonoBehaviour
{
    [SerializeField] private Volume globalVolume;
    [Header("Bloom (Sternen-Glow)")]
    [SerializeField] private float baseBloom = 1.2f;


    [Header("Bloom (Sternen-Glow)")]
    [SerializeField] private float boostBloom = 3.0f;

    [Header("Vignette (Spannung/ Tunnelblick)")]
    [SerializeField] private float baseVignette = 0.2f;
    [SerializeField] private float maxVignette = 0.48f;

    private Bloom bloom;
    private Vignette vignette;
    private float survivalTime = 0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
     if (globalVolume == null) globalVolume = GetComponent<Volume>(); 

     if(globalVolume != null && globalVolume.profile != null)
     {
        globalVolume.profile.TryGet(out bloom);
        globalVolume.profile.TryGet(out vignette);
     }   

    }

    // Update is called once per frame
    void Update()
    {
     survivalTime += Time.deltaTime;
     bool isBooting = Mouse.current != null && Mouse.current.leftButton.isPressed;

     if(bloom != null)
     {
        float targetBloom = baseBloom;
        if (isBooting)
        {
            targetBloom = boostBloom;
        }
        
        bloom.intensity.value = Mathf.Lerp(bloom.intensity.value, targetBloom, Time.deltaTime * 6f);
     }

     if (vignette != null)
     {
        float timeFactor = Mathf.Clamp01(survivalTime/ 60f);
        float targetVignette = Mathf.Lerp(baseVignette, maxVignette, timeFactor);
        if (isBooting) targetVignette += 0.08f;

        vignette.intensity.value = Mathf.Lerp(vignette.intensity.value, targetVignette, Time.deltaTime * 4f);
     }   
    }
}
