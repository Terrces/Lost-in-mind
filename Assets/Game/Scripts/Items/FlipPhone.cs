using UnityEngine;

public class FlipPhone : MonoBehaviour, Iusable
{
    public Canvas Canvas;
    public Canvas ScreenCanvas;
    public Light screenLight;

    void Start()
    {
        Canvas.worldCamera = Camera.main;
        SetState(false);
    }

    private void SetState(bool isEnable)
    {
        Canvas.enabled = isEnable;
        ScreenCanvas.enabled = isEnable;
        screenLight.enabled = isEnable;

        if (isEnable)
        {
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    [ContextMenu("off phone screen")]
    public void OffScreen() => SetState(false);

    public void Use()
    {
        SetState(true);
    } 
}
