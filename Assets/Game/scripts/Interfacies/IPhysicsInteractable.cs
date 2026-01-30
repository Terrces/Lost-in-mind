using UnityEngine;

public interface IPhysicsInteractable
{
    public bool Interactable {get; set;}
    public GameObject Interact();
}
