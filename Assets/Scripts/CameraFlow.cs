using UnityEngine;

public class CameraFlow : MonoBehaviour
{
    public GameObject targetObject;
    private Vector3 targetObjectPosition;
    private Vector3 distanceToTarget;

    private void Start()
    {
        targetObjectPosition = targetObject.transform.position;
        distanceToTarget = transform.position - targetObjectPosition;
    }

    private void LateUpdate()
    {
        Vector3 newCameraPosition = transform.position;
        newCameraPosition = targetObject.transform.position + distanceToTarget;
        transform.position = newCameraPosition;
    }
}
