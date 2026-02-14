using UnityEngine;

public class LookAt : MonoBehaviour
{
    public GameObject _obj;
    private Player player => GetComponent<Player>();
    private float xRotation = 0f;

    public float SmoothX = 3f;
    public float SmoothY = 3f;
    void Start()
    {
        if (_obj)
        {
            player.lookAvailable = false;
        }
    }

    void Update()
    {
        if (_obj == null)
        {
            player.lookAvailable = true;
            return;
        }


        Vector3 cameraPos = player.cameraTransform.position;
        Vector3 dirToTarget = (_obj.transform.position - cameraPos).normalized;

        float targetPitch = -Mathf.Rad2Deg * Mathf.Atan2(dirToTarget.y, new Vector2(dirToTarget.x, dirToTarget.z).magnitude);
        xRotation = Mathf.Clamp(Mathf.LerpAngle(xRotation, targetPitch, Time.deltaTime * SmoothX), -90, 90);

        player.cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        
        Vector3 flatTarget = new Vector3(_obj.transform.position.x, transform.position.y, _obj.transform.position.z);
        Vector3 flatDir = (flatTarget - transform.position).normalized;

        Quaternion targetBodyRot = Quaternion.LookRotation(flatDir);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetBodyRot, Time.deltaTime * SmoothY);
    }
}
