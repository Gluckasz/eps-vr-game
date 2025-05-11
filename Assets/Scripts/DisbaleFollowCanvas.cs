using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.UI;

public class LazyFollowDisabler : MonoBehaviour
{
    private LazyFollow lazyFollow;

    void Start()
    {
        lazyFollow = GetComponent<LazyFollow>();
        if (lazyFollow != null)
        {
            // Make sure it's set to Follow right away
            lazyFollow.positionFollowMode = LazyFollow.PositionFollowMode.Follow;

            // Disable it shortly after
            StartCoroutine(DisableLazyFollowAfterDelay(0.1f));
        }
    }

    System.Collections.IEnumerator DisableLazyFollowAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Stop following by switching mode to None!
        lazyFollow.positionFollowMode = LazyFollow.PositionFollowMode.None;
    }
}
