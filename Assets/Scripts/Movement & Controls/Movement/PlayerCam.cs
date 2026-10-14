using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCam : MonoBehaviour
{
    [SerializeField] private float sensX = 0.1f;
    [SerializeField] private float sensY = 0.1f;
    [SerializeField] private Transform orientation;

    [Header("Crouch")]
    [SerializeField] private float crouchLerpSpeed = 10f;
    private float targetYHeight;
    private bool targetHeightSet = false;

    private float xRotation;
    float yRotation;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        if (Mouse.current == null)
            return;

        Vector2 mouseDelta = Mouse.current.delta.ReadValue();

        float mouseX = mouseDelta.x * sensX;
        float mouseY = mouseDelta.y * sensY;

        yRotation += mouseX;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // rotate cam and orientation
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);

        if (orientation != null)
            orientation.rotation = Quaternion.Euler(0f, yRotation, 0f);

        // smoothly move toward crouch/stand height
        if (targetHeightSet)
        {
            Vector3 pos = transform.localPosition;
            pos.y = Mathf.Lerp(pos.y, targetYHeight, crouchLerpSpeed * Time.deltaTime);
            transform.localPosition = pos;
        }
    }

    public void SetCrouchHeight(float yOffset)
    {
        targetYHeight = yOffset;
        targetHeightSet = true;
    }
}
