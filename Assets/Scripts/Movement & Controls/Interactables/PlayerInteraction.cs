using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Detection")]
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float range = 5f;
    [SerializeField] private LayerMask interactableLayers;

    [Header("UI")]
    [SerializeField] private TMP_Text promptText;

    private IInteractable currentInteractable;

    public void Awake()
    {
        if (playerCamera == null)
            playerCamera = Camera.main;

        if (playerCamera == null)
        {
            Debug.LogError("PlayerInteraction: no camera assigned and no MainCamera found.", this);
            enabled = false;
        }
    }

    private void Update()
    {
        FindInteractable();
        HandleInput();
        UpdatePrompt();
    }

    private void FindInteractable()
    {
        currentInteractable = null;

        Transform cam = playerCamera.transform;

        if (Physics.Raycast(cam.position, cam.forward, out RaycastHit hit, range,
                interactableLayers, QueryTriggerInteraction.Ignore))
        {
            currentInteractable = hit.collider.GetComponentInParent<IInteractable>();
        }
    }

    private void HandleInput()
    {
        if (currentInteractable == null)
            return;

        var keyboard = Keyboard.current;
        if (keyboard != null && keyboard.eKey.wasPressedThisFrame)
            currentInteractable.Interact(gameObject);
    }

    private void UpdatePrompt()
    {
        if (promptText == null)
            return;

        string newText = currentInteractable != null ? currentInteractable.InteractionText : "";

        if (promptText.text != newText)
            promptText.text = newText;
    }
}
