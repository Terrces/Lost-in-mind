using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MainGUI : MonoBehaviour
{
    [SerializeField] private List<GameObject> Items;
    [SerializeField] private Transform point;

    private GameObject currentObject;

    private int tempCurrentInd;

    public void ToggleItem()
    {
        SetItemIcon(tempCurrentInd);

        tempCurrentInd = (tempCurrentInd += 1) % Items.Count;
    }

    public void SetItemIcon(int index)
    {
        if (currentObject)
        {
            Destroy(currentObject);            
        }
        if (Items[index])
        {
            currentObject = Instantiate(Items[index], point);
        }
    }
}
