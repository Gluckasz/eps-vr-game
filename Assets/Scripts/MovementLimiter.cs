using UnityEngine;

/// <summary>
/// Restricts the movement of this GameObject along the Y-axis.
/// If Y-axis displacement is too high, then the movement is abandoned
/// </summary>
public class MovementLimiter : MonoBehaviour
{
    /// <summary>
    /// The maximum allowed change in the Y-coordinate per frame.
    /// </summary>
    [Tooltip("The maximum allowed change in the Z-coordinate per frame.")]
    public float maxYChangePerFrame = 0.1f;

    private Vector3 previousPosition;

    void Start()
    {
        previousPosition = transform.position;
    }

    void LateUpdate()
    {
        Vector3 currentPosition = transform.position;

        float actualYChange = currentPosition.y - previousPosition.y;

        if (Mathf.Abs(actualYChange) > maxYChangePerFrame)
        {
            transform.position = previousPosition;
            Debug.Log("Step is too high- staying at the position: " + transform.position);
        }

        previousPosition = transform.position;
    }
}
