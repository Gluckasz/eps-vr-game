using Unity.VRTemplate;
using Unity.XR.CoreUtils;
using UnityEngine;

public class LocomotionDisabler : MonoBehaviour
{

    public bool enableMovement = true;
    public bool enableTurning = true;

    public GameObject move;
    public GameObject turn;

    public GameObject affordanceCalloutMove;
    public GameObject affordanceCalloutTurn;

    public GameObject joystickAffordancesMove;
    public GameObject joystickAffordancesTurn;

    void Update()
    {
        move.SetActive(enableMovement);
        turn.SetActive(enableTurning);

        SetGameObjectInvisible(joystickAffordancesMove, enableMovement);
        SetGameObjectInvisible(joystickAffordancesTurn, enableTurning);

        SetGameObjectInvisible(affordanceCalloutMove, enableMovement);
        SetGameObjectInvisible(affordanceCalloutTurn, enableTurning);
    }

    private void SetGameObjectInvisible(GameObject parent, bool active)
    {
        for (int i = 0; i < parent.transform.childCount; i++)
        {
            parent.transform.GetChild(i).gameObject.SetActive(active);
        }

        parent.GetComponent<Callout>().enabled = active;

    }
}
