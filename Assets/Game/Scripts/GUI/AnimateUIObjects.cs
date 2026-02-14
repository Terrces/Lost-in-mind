using UnityEngine;

public class AnimateUIObjects : MonoBehaviour
{
    enum Axis {BOTH, X, XY, XZ, Y, YZ, Z}
    [SerializeField] private Axis AnimateAxis; 
    public float speed = 1.0f;

    private Vector3 currentEuler ;

    void Start()
    {
        currentEuler = transform.rotation.eulerAngles;
    }

    void FixedUpdate()
    {
        float delta = speed * Time.deltaTime;

        switch (AnimateAxis)
        {
            case Axis.BOTH:
                currentEuler += new Vector3(delta, delta, delta);
                break;
            case Axis.X:
                currentEuler.x += delta;
                break;
            case Axis.XY:
                currentEuler.x += delta;
                currentEuler.y += delta;
                break;
            case Axis.XZ:
                currentEuler.x += delta;
                currentEuler.z += delta;
                break;
            case Axis.Y:
                currentEuler.y += delta;
                break;
            case Axis.YZ:
                currentEuler.y += delta;
                currentEuler.z += delta;
                break;
            case Axis.Z:
                currentEuler.z += delta;
                break;
        }

        transform.rotation = Quaternion.Euler(currentEuler);
    }
}
