using UnityEngine;

public interface IInteractable
{
        public string InteractionText { get; }
        void Interact(GameObject interactor);
}
