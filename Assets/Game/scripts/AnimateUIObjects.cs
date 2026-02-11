using UnityEngine;

public class AnimateUIObjects : MonoBehaviour
{
    enum Axis {BOTH, X, XY, XZ, Y, YZ, Z}
    [SerializeField] private Axis AnimateAxis; 
    public float speed = 1.0f;

    void FixedUpdate()
    {
        switch (AnimateAxis)
        {
            case Axis.BOTH:
                gameObject.transform.Rotate(new Vector3(transform.rotation.x + speed, transform.rotation.x + speed, transform.rotation.z + speed));
            break;
            
            case Axis.X:
                gameObject.transform.Rotate(new Vector3(transform.rotation.x + speed, 0, 0));
            break;
            
            case Axis.XY:
                gameObject.transform.Rotate(new Vector3(transform.rotation.x + speed, transform.rotation.y + speed, 0));
            break;
            
            case Axis.XZ:
                gameObject.transform.Rotate(new Vector3(transform.rotation.x + speed, 0, transform.rotation.z + speed));
            break;

            case Axis.Y:
                gameObject.transform.Rotate(new Vector3(0, transform.rotation.y + speed, 0));
            break;
            
            case Axis.YZ:
                gameObject.transform.Rotate(new Vector3(0, transform.rotation.y + speed, transform.rotation.z + speed));
            break;

            case Axis.Z:
                gameObject.transform.Rotate(new Vector3(0, 0, transform.rotation.z + speed));
            break;
        }
    }
}
