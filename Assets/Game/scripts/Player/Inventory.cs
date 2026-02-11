using UnityEngine;

public class Inventory : MonoBehaviour
{
    // List<GameObject> items = new List<GameObject>();
    public Transform Point;
    private GameObject currentItem;
    public bool HideCurrentItem;
    private int currentItemIdx;

    public MainGUI gui;
    private int toggleCounts;

    void Start()
    {
        gui = FindFirstObjectByType<MainGUI>();
        ToggleItem();
    }

    public void ToggleItem()
    {
        Debug.Log(toggleCounts);
        if(toggleCounts == 1)
        {
            HideCurrentItem = false;
            if (!GetComponent<Interaction>().ObjectIsCarried)
            {
                RestoreItem();
            }
        }
        else
        {
            HideCurrentItem = true;
            HideItem();
        }

        gui.ToggleItem();

        toggleCounts += 1;

        if(toggleCounts > 2)
        {
            toggleCounts = 0;
        }
    }

    public void UseItem()
    {
        if (currentItem != null && currentItem.TryGetComponent(out Iusable use))
        {
            use.Use();
        }
    }

    public GameObject GetCurrentItem()
    {
        if(currentItem == null) return null;
        return currentItem;
    }

    public bool CheckAvailable(GameObject _item)
    {
        for (int i = 0; i < InventoryHandler.Items.Count; i++)
        {
            if (InventoryHandler.Items[i] != _item)
            {
                continue;
            }
            else
            {
                return false;
            }
        }

        return true;
    }

    public void AddItem(GameObject _item)
    {
        if(!_item) return;
        
        InventoryHandler.Items.Add(_item);

        for (int i = 0; i < InventoryHandler.Items.Count; i++)
        {
            if (InventoryHandler.Items[i] == _item)
            {
                GetItem(i);
            }
        }
    }

    public void GetItem(int value)
    {
        currentItemIdx = value;
        currentItem = Instantiate(InventoryHandler.Items[value],Point.transform);
        if(HideCurrentItem) Destroy(currentItem);
    }

    public void RestoreItem()
    {
        if(InventoryHandler.Items.Count == 0 || HideCurrentItem) return;
        if(currentItem == null && InventoryHandler.Items[currentItemIdx] != null)
        {
            currentItem = Instantiate(InventoryHandler.Items[currentItemIdx],Point.transform);
        }
    }

    public void HideItem()
    {
        if (currentItem != null && currentItem.TryGetComponent(out Animator animator))
        {
            animator.Play("Hide");
            currentItem = null;
        }
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}
