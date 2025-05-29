using UnityEngine;

public class InspectableItem : MonoBehaviour
{
    public string itemName;
    [TextArea]
    public string description;
    [TextArea]
    public string fullDescription;
    public Sprite itemImage;
    public Transform itemPosition;
}
