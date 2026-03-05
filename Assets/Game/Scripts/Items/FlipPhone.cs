using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class FlipPhone : MonoBehaviour, Iusable
{
    public Canvas Canvas;
    public Canvas ScreenCanvas;
    public Light screenLight;
    private Animator animator => GetComponent<Animator>();
    private bool animationAvailable = false;
    [SerializeField] private GameObject canvasContainer;
    public Vector3 StartCanvasRotation;
    private Player player;
    private float offScreenCanvasHideTime = 0.1f;

    void Start()
    {
        Canvas.worldCamera = Camera.main;
        player = FindFirstObjectByType<Player>();
        SetStateScreen(false);
        animationAvailable = true;
        canvasContainer.transform.Rotate(StartCanvasRotation);
    }

    private IEnumerator offScreen()
    {
        yield return new WaitForSeconds(offScreenCanvasHideTime);
        ScreenCanvas.enabled = false;
    }

    private bool isOpen;
    private void SetScreenAnimation(bool isEnable)
    {
        if (isOpen != isEnable)
        {
            isOpen = isEnable;
            
            if (isEnable && animationAvailable)
            {
                animator.Play("Open");
                ScreenCanvas.enabled = true;
                canvasContainer.transform.DORotate(new Vector3(0,0,0),0.15f);
                player.PlayerWalk = true;
                player.SecondarySensitivity = true;
            }
            else if(animationAvailable)
            {
                animator.Play("Close");
                canvasContainer.transform.DORotate(StartCanvasRotation, offScreenCanvasHideTime);
                StartCoroutine(offScreen());
                player.PlayerWalk = false;
                player.SecondarySensitivity = false;
            }
        }
    }

    private void SetStateScreen(bool isEnable)
    {
        Canvas.enabled = isEnable;
        screenLight.enabled = isEnable;
    }


    public void Hide()
    {
        animationAvailable = false;
        SetStateScreen(false);
        canvasContainer.transform.DORotate(StartCanvasRotation, offScreenCanvasHideTime);
        StartCoroutine(offScreen());
        Cursor.lockState = CursorLockMode.Locked;
        player.SecondarySensitivity = false;
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
        SetStateScreen(false);
        Cursor.lockState = CursorLockMode.Locked;
        player.SecondarySensitivity = false;
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}
