using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public int roomNumber;
    public Transform PackagePoint;
    public Light _light;

    public List<GameObject> doorModels;

    void Start()
    {
        chooseModel();
    }

    void chooseModel()
    {
        if(doorModels.Count == 0) return;

        int value = Random.Range(0,doorModels.Count);

        doorModels[value].SetActive(true);
        
        for (int i = 0; i < doorModels.Count; i++)
        {
            if (i == value || doorModels[i] == null)
            {
                continue;
            }

            if(i != value || doorModels[i] != null)
            {
                Destroy(doorModels[i]);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out Package package))
        {
            if (package.RoomNumber == roomNumber)
            {
                package.Delivered();
                _light.enabled = false;
                if (PackagePoint != null && package.TryGetComponent(out Rigidbody rigidbody))
                {
                    StartCoroutine(delivered(package));
                }
                else
                {
                    Destroy(package.gameObject);
                }
            }
        }
    }

    IEnumerator delivered(Package package)
    {
        FindFirstObjectByType<Interaction>().DropObject();
        package.Interactable = false;
        yield return null;
    }
}
