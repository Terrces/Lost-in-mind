using UnityEngine;

public class TutorialWoodenBox : Destroyable
{
    [SerializeField] private SledgehammerScene scene;
    void Start()
    {
        OnDestroyed += TakeDamage;
    }
    public void TakeDamage()
    {
        scene.EndScene();
    }
}
