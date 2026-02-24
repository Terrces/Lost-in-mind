using UnityEngine;

public class FlipPhone : MonoBehaviour, Iusable
{
    public Canvas Canvas;
    public Canvas ScreenCanvas;
    public Light screenLight;
    private Animator animator => GetComponent<Animator>();
    private bool animationAvailable = false;

    void Start()
    {
        Canvas.worldCamera = Camera.main;
        SetStateScreen(false);
        animationAvailable = true;
    }

    private void SetScreenAnimation(bool isEnable)
    {
        if (isEnable && animationAvailable)
        {
            animator.Play("Open");
        }
        else if(animationAvailable)
        {
            animator.Play("Close");
        }
    }

    private void SetStateScreen(bool isEnable)
    {
        Canvas.enabled = isEnable;
        ScreenCanvas.enabled = isEnable;
        screenLight.enabled = isEnable;
    }


    public void Hide()
    {
        animationAvailable = false;
        SetStateScreen(false);
        Cursor.lockState = CursorLockMode.Locked;

    }

    public void Use()
    {
        SetScreenAnimation(true);
        Cursor.lockState = CursorLockMode.None;

    }

    [ContextMenu("off phone screen")]
    public void OffScreen() => SetStateScreen(false);
    public void OnScreen() => SetStateScreen(true);

    public void Close()
    {
        SetScreenAnimation(false);
        OffScreen();
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}
