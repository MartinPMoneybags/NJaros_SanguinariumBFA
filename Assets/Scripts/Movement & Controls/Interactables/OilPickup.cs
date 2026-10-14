using UnityEngine;

public class OilPickup : MonoBehaviour, IInteractable
{
    [SerializeField] private string interactionText = "[E] Pickup oil flask";
    [SerializeField] private int flaskAmount = 1;
    [SerializeField] private AudioClip pickupSound;
    [SerializeField, Range(0f, 1f)] private float pickipVolume = 1f;

    public string InteractionText => interactionText;

    public void Interact(GameObject interactor)
    {
        Lantern lantern = FindFirstObjectByType<Lantern>();

        if (lantern == null)
        {
            Debug.LogWarning("OilPickup: no Lantern found in the scene.", this);
            return;
        }

        lantern.AddFlasks(flaskAmount);

        if (pickupSound != null)
            AudioSource.PlayClipAtPoint(pickupSound, transform.position, pickipVolume);

        Destroy(gameObject);
    }
}
