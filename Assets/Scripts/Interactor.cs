using UnityEngine;

public class Interactor : MonoBehaviour
{
    public float interactDistance = 5f; // Tinaasan natin paitaas para siguradong abot

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, interactDistance);

            foreach (var hitCollider in hitColliders)
            {
                // Hahanapin ang IInteractable sa mismong object o sa parent/children nito
                IInteractable interactable = hitCollider.GetComponentInParent<IInteractable>();
                if (interactable == null)
                {
                    interactable = hitCollider.GetComponentInChildren<IInteractable>();
                }

                if (interactable != null)
                {
                    interactable.Interact();
                    break;
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, interactDistance);
    }
}