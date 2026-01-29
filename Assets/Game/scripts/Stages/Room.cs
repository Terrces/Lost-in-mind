using UnityEngine;

public class Room : MonoBehaviour
{
    public int roomNumber;

    void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out Package package))
        {
            if (package.roomNumber == roomNumber)
            {
                if(other.TryGetComponent(out Destroyable destroyable))
                {
                    destroyable.Destroy();
                }
            }
        }
    }
    void OnTriggerExit(Collider other)
    {
        
    }

}
