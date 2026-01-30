using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SledgehammerScene : MonoBehaviour, Iinteractable
{
    public InteractionObjectTypes types {get;set;} = InteractionObjectTypes.Item;
    [SerializeField] private Light _light;
    public GameObject Item;
    public GameObject cutSceneCamera;
    public GameObject PlayerCamera;
    public GameObject Player;
    public GameObject Hammer;
    private Animator cutSceneAnimation => GetComponent<Animator>();

    private Vector3 startCameraPosition;
    private Vector3 startCutCameraPosition;

    public void Interact()
    {
        startCameraPosition = PlayerCamera.transform.position;
        startCutCameraPosition = cutSceneCamera.transform.position;
        
        Player.GetComponent<Player>().movingAvailable = false;
        Player.SetActive(false);
        cutSceneCamera.SetActive(true);
        cutSceneCamera.transform.position = startCameraPosition;
        cutSceneCamera.transform.DOMove(startCutCameraPosition,0.5f);
        
        cutSceneAnimation.Play("PickupSledgehammer");
    }

    IEnumerator EndRoutine()
    {
        yield return new WaitForSeconds(0.5f);
        _light.DOColor(Color.black, 1.5f);
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadSceneAsync("Game");
    }

    public void EndScene()
    {
        StartCoroutine(EndRoutine());
    }

    public void OnComplited()
    {
        startCameraPosition = PlayerCamera.transform.position;

        cutSceneCamera.SetActive(false);
        Player.SetActive(true);

        Player.GetComponent<Player>().movingAvailable = true;
        Player.GetComponent<Inventory>().AddItem(Item);

        Destroy(GetComponent<Collider>());
        Destroy(Hammer);
    }
}
