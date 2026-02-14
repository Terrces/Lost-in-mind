public enum InteractionObjectTypes {Object, Item}

public interface Iinteractable
{
    public InteractionObjectTypes types { get; set; }
    public void Interact();
}
