using UnityEngine;

public class InteractableObject : MonoBehaviour, Iinteractable
{
    [SerializeField] private string Hint = "";
    public InteractionObjectTypes types { get; set; } = InteractionObjectTypes.Object;
    [SerializeField] private Interactable OtherInteractableObject;

    public void Interact()
    {
        if (OtherInteractableObject != null)
        {
            OtherInteractableObject.Interact();
        }
        else
        {
            Debug.LogWarning("Please setup Interactable");
        }
    }
}
