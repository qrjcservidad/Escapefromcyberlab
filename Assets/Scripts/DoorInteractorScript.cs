using UnityEngine;

public class DoorInteractor : MonoBehaviour
{
    [SerializeField] private float interactRange = 3f;
    [SerializeField] private LayerMask interactableLayer;

    // Ito ang function na tinatawag ng UI [F] Button OnClick()
    public void InteractWithNearby()
    {
        // Mag-search ng colliders sa paligid ng Player
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, interactRange);

        foreach (var hitCollider in hitColliders)
        {
            // 1. Check kung Door ang malapit
            DoorController door = hitCollider.GetComponent<DoorController>();
            if (door != null)
            {
                door.ToggleDoor();
                return; // Huminto na kapag nakakita ng pinto
            }

            // 2. Check kung KeyPickup ang malapit
            KeyPickup key = hitCollider.GetComponent<KeyPickup>();
            if (key != null)
            {
                key.PickUpKey();
                return; // Huminto na kapag nakakolekta ng susi
            }

            // 3. Check kung AccessCardPickup ang malapit (para sa card_1, card_2, etc.)
            AccessCardPickup card = hitCollider.GetComponent<AccessCardPickup>();
            if (card != null)
            {
                card.OnPickupButtonPressed();
                return; // Huminto na kapag nakakolekta ng access card
            }
        }
    }

    // Visual Gizmo sa Scene view para makita ang interaction radius
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactRange);
    }
}