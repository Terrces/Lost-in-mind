using System;
using UnityEngine;

public class Interactable : MonoBehaviour, Iinteractable
{
    public event Action Interacted;
    public InteractionObjectTypes types { get; set; } = InteractionObjectTypes.Object;

    public void Interact()
    {
        Interacted?.Invoke();
    }
}
