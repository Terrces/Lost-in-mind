using System.Collections.Generic;
using UnityEngine;

public class SceneProperties : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private Inventory inventory;

    [SerializeField] private bool enableStartItems = true;
    [SerializeField] private List<GameObject> startItems;

    void Start()
    {
        if(startItems.Count != 0 && inventory != null && enableStartItems)
        {
            foreach (GameObject _obj in startItems)
            {
                inventory.AddItem(_obj, false);
            }
        }
    }

}
