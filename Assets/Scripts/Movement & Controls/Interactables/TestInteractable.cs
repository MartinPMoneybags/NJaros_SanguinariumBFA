using UnityEngine;

public class TestInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private string interactionText = "[E] to interact";

    public string InteractionText => interactionText;

    public void Interact(GameObject interactor)
    {
        Debug.Log($"{interactor.name} interacted with {name}!");
    }
}
