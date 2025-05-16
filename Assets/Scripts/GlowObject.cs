using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class AlwaysGlow : MonoBehaviour
{
    public Color glowColor = Color.white;
    public float glowIntensity = 1.5f;

    private Material glowMat;

    void Start()
    {
        Renderer rend = GetComponent<Renderer>();

        // Duplicate the material so each object can glow independently
        glowMat = new Material(rend.material);
        glowMat.EnableKeyword("_EMISSION");
        glowMat.SetColor("_EmissionColor", glowColor * glowIntensity);

        rend.material = glowMat;
    }
}
