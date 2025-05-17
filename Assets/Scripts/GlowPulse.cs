using UnityEngine;

public class PulseGlow : MonoBehaviour
{
    public float pulseSpeed = 2f;
    public float minIntensity = 0.5f;
    public float maxIntensity = 2f;
    private Material mat;
    private float t;

    void Start()
    {
        mat = GetComponent<Renderer>().material;
    }

    void Update()
    {
        t += Time.deltaTime * pulseSpeed;
        float intensity = Mathf.Lerp(minIntensity, maxIntensity, (Mathf.Sin(t) + 1f) / 2f);
        mat.SetColor("_EmissionColor", mat.color * intensity);
    }
}
