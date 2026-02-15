using UnityEngine;

public class Inventory : MonoBehaviour
{
    public Transform Point;
    private GameObject currentItem;
    public bool HideCurrentItem;
    private int currentItemIdx;
    private int currentSlotIndex = 0;

    public MainGUI gui;


    void Start()
    {
        gui = FindFirstObjectByType<MainGUI>();
        ToggleItem();
    }

    public void ToggleItem()
    {
        currentSlotIndex += 1;

        if(currentSlotIndex == InventoryHandler.Items.Count + 1)
        {
            currentSlotIndex = 0;
        }
        
        if (currentSlotIndex == 0)
        {
            Debug.Log("Hide Item");
            HideCurrentItem = true;
            HideItem();
        }
        else
        {
            int itemIndex = currentSlotIndex-1;
            HideCurrentItem = false;
            HideItem();
            GetItem(itemIndex);
        }

        gui.SetItemIcon(currentSlotIndex);
    }

    public void UseItem()
    {
        if (currentItem != null && currentItem.TryGetComponent(out Iusable use)) use.Use();
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
            if (InventoryHandler.Items[i] != _item) continue;
            else return false;
        }

        return true;
    }

    public void AddItem(GameObject _item)
    {
        if(!_item) return;
        
        InventoryHandler.Items.Add(_item);

        for (int i = 0; i < InventoryHandler.Items.Count; i++)
        {
            if (InventoryHandler.Items[i] == _item) GetItem(i);
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
        else
        {
            Destroy(currentItem);
            currentItem = null;
        }
    }

    /// <summary>
    /// Function for animation player
    /// </summary>
    public void Destroy() => Destroy(gameObject);
}
