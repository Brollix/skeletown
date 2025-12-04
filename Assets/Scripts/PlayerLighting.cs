using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PlayerLighting : MonoBehaviour
{
    [Header("Light Settings")]
    [SerializeField] private Color lightColor = new Color(1f, 0.9f, 0.7f); // Warm light
    [SerializeField] private float intensity = 1.0f;
    [SerializeField] private float radius = 5.0f;
    [SerializeField] private bool enableFlicker = true;

    private Light2D playerLight;
    private float baseIntensity;

    private void Start()
    {
        // Try to get existing Light2D or add a new one
        playerLight = GetComponent<Light2D>();
        if (playerLight == null)
        {
            playerLight = gameObject.AddComponent<Light2D>();
        }

        // Configure the light
        playerLight.color = lightColor;
        playerLight.intensity = intensity;
        playerLight.pointLightOuterRadius = radius;
        playerLight.pointLightInnerRadius = radius * 0.2f;
        playerLight.lightType = Light2D.LightType.Point;

        baseIntensity = intensity;
    }

    private void Update()
    {
        if (enableFlicker && playerLight != null)
        {
            // Simple flicker effect
            float noise = Mathf.PerlinNoise(Time.time * 2f, 0f);
            playerLight.intensity = baseIntensity + (noise * 0.2f - 0.1f);
        }
    }
}
