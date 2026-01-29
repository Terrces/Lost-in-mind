using UnityEngine;

[SelectionBase]
public class billbord : MonoBehaviour
{
    private Transform cameraTransform;

    void Start() => cameraTransform = Camera.main.transform;

    void Update() => transform.LookAt(cameraTransform.position + cameraTransform.forward);
}
