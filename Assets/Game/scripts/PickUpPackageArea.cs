using System.Collections.Generic;
using UnityEngine;

public class PickUpPackageArea : MonoBehaviour, Iinteractable
{
    public List<GameObject> objects;
    public Stage stage;
    private int previouslyRoomNumber = 0;

    public InteractionObjectTypes types {get;set;} = InteractionObjectTypes.Object;

    public void Interact()
    {
        GameObject _obj = Instantiate(objects[0],transform.position,Quaternion.Euler(Vector3.zero), FindAnyObjectByType<Stage>().transform); 
        Interaction interact = FindAnyObjectByType<Interaction>();
        if(_obj.TryGetComponent(out Package package))
        {
            package.roomNumber = Random.Range(1,stage.maxRoomNumber+1);

            previouslyRoomNumber = package.roomNumber;
        }
        interact.PickUpPhysicsObjects(_obj.GetComponent<IPhysicsInteractable>());
    }
}
