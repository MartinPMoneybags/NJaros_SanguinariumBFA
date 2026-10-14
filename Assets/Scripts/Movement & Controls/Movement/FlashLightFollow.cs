using UnityEngine;

public class FlashLightFollow : MonoBehaviour
{
    [SerializeField] private Transform target; //drag PlayerCamera here
    [SerializeField] private float rotationSpeed = 12f;

    private void LateUpdate()
    {
        if (target == null)
            return;

        transform.position = target.position;
        transform.rotation = Quaternion.Slerp(transform.rotation, target.rotation, rotationSpeed * Time.deltaTime);
    }
}
