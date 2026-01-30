using UnityEngine;

public class TutorialWoodenBox : Destroyable, Idamageable
{
    [SerializeField] private SledgehammerScene scene;
    public void TakeDamage(int value)
    {
        scene.EndScene();
        Destroy();
    }
}
