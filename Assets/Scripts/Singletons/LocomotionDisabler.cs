using Unity.VRTemplate;
using Unity.XR.CoreUtils;
using UnityEngine;

public class LocomotionDisabler : MonoBehaviour
{
    public static LocomotionDisabler Instance { get; private set; }

    public bool enableMovement = true;
    public bool enableTurning = true;

    public GameObject move;
    public GameObject turn;

    public GameObject affordanceCalloutMove;
    public GameObject affordanceCalloutTurn;

    public GameObject joystickAffordancesMove;
    public GameObject joystickAffordancesTurn;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(Instance.gameObject);
        }
        Instance = this;
    }

    private void Update()
    {
        affordanceCalloutMove.GetComponent<Callout>().enabled = enableMovement;
        affordanceCalloutTurn.GetComponent<Callout>().enabled = enableTurning;
        move.SetActive(enableMovement);
        turn.SetActive(enableTurning);

        SetChildrenInactive(joystickAffordancesMove, enableMovement);
        SetChildrenInactive(joystickAffordancesTurn, enableTurning);

        SetChildrenInactive(affordanceCalloutMove, enableMovement);
        SetChildrenInactive(affordanceCalloutTurn, enableTurning);
    }

    private void SetChildrenInactive(GameObject parent, bool active)
    {
        for (int i = 0; i < parent.transform.childCount; i++)
        {
            GameObject child = parent.transform.GetChild(i).gameObject;
            if (active && !child.activeInHierarchy && child.name == "Curve")
            {
                continue;
            }
            child.SetActive(active);
        }
    }
}
