using UnityEngine;

public class GrabbableGlowLight : MonoBehaviour
{
    public Color glowColor = Color.cyan;
    public float intensity = 1.2f;
    public float range = 2f;
    public float glowPulseSpeed = 2f; // Optional: for breathing/pulse effect

    private Light glowLight;

    void Start()
    {
        // Create the light source
        glowLight = gameObject.AddComponent<Light>();
        glowLight.type = LightType.Point;
        glowLight.color = glowColor;
        glowLight.intensity = intensity;
        glowLight.range = range;

        // Make it subtle and VR-friendly
        glowLight.shadows = LightShadows.None;
    }

    void Update()
    {
        // Optional: pulsing glow effect
        glowLight.intensity = intensity + Mathf.Sin(Time.time * glowPulseSpeed) * 0.2f;
    }
}
