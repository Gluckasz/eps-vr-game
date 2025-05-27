using UnityEngine;

public class SnapGhost : MonoBehaviour
{
    public SnapToGhost.SnapType ghostType;

    private void Start()
    {
        if (ghostType == SnapToGhost.SnapType.Glass)
            SnapToGhost.allGhostsGlass.Add(transform);
        else
            SnapToGhost.allGhostsDish.Add(transform);
    }

    private void OnDestroy()
    {
        // Clean up if ghost gets deleted
        if (ghostType == SnapToGhost.SnapType.Glass)
            SnapToGhost.allGhostsGlass.Remove(transform);
        else
            SnapToGhost.allGhostsDish.Remove(transform);
    }
}
