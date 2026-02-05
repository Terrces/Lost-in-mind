using UnityEngine;

public class PhysicalObject : Destroyable, IPhysicsInteractable
{
    public Vector3 offset = new Vector3(0,0,1);
    public bool Interactable {get; set;} = true;
    public GameObject Interact()
    {
        return gameObject;
    }
}
