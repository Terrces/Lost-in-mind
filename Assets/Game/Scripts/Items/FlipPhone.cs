using System.Collections;
using DG.Tweening;
using UnityEngine;

public class FlipPhone : MonoBehaviour, Iusable
{
    private const float OPEN_ANIMATION_DURATION = 0.15f;
    private const float CLOSE_ANIMATION_DURATION = 0.1f;

    [SerializeField] public Canvas Canvas;
    [SerializeField] public Canvas ScreenCanvas;
    [SerializeField] public Light screenLight;
    [SerializeField] private GameObject canvasContainer;
    [SerializeField] public Vector3 StartCanvasRotation;

    private Animator animator;
    private Player player;
    private bool animationAvailable = false;
    private bool isOpen = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        Canvas.worldCamera = Camera.main;
        player = FindFirstObjectByType<Player>();
        SetStateScreen(false);
        animationAvailable = true;
        canvasContainer.transform.Rotate(StartCanvasRotation);
        canvasContainer.SetActive(false);
    }

    private IEnumerator HideScreenDelayed()
    {
        yield return new WaitForSeconds(CLOSE_ANIMATION_DURATION);
        ScreenCanvas.enabled = false;
    }

    private void SetPlayerState(bool allowMovement)
    {
        if (player != null)
        {
            player.PlayerWalk = allowMovement;
            player.SecondarySensitivity = allowMovement;
        }
    }

    private void SetScreenAnimation(bool isEnable)
    {
        if (isOpen == isEnable || !animationAvailable)
            return;

        isOpen = isEnable;
        
        if (isEnable)
        {
            animator.Play("Open");
            ScreenCanvas.enabled = true;
            canvasContainer.SetActive(true);
            canvasContainer.transform.DORotate(Vector3.zero, OPEN_ANIMATION_DURATION);
            SetPlayerState(true);
        }
        else
        {
            animator.Play("Close");
            canvasContainer.transform.DORotate(StartCanvasRotation, CLOSE_ANIMATION_DURATION);
            StartCoroutine(HideScreenDelayed());
            SetPlayerState(false);
        }
    }

    private void SetStateScreen(bool isEnable)
    {
        Canvas.enabled = isEnable;
        if (screenLight != null)
            screenLight.enabled = isEnable;
    }

    public void Hide()
    {
        animationAvailable = false;
        SetStateScreen(false);
        canvasContainer.transform.DORotate(StartCanvasRotation, CLOSE_ANIMATION_DURATION);
        StartCoroutine(HideScreenDelayed());
        Cursor.lockState = CursorLockMode.Locked;
        SetPlayerState(false);
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
        SetPlayerState(false);
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}
