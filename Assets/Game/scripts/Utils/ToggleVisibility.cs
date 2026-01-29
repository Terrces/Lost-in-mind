using UnityEngine;

public class ToggleVisibility : MonoBehaviour
{
    public GameObject obj;
    public bool ShowOnly = false;
    public bool HideOnly = false;
    void OnTriggerEnter(Collider other)
    {
        if(other.tag != "Player") return;

        if(ShowOnly) return;
        if(obj.activeInHierarchy) obj.SetActive(false);
    }

    void OnTriggerExit(Collider other)
    {
        if(other.tag != "Player") return;
        
        if(HideOnly) return;
        if(obj.activeInHierarchy) obj.SetActive(true);
    }
}
