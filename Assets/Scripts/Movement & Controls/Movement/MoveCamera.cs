using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    [SerializeField] private Transform cameraPosition;

    private void LateUpdate()
    {
        if (cameraPosition != null)
            transform.position = cameraPosition.position;
    }
}
