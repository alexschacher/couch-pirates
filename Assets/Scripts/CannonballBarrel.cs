using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonballBarrel : MonoBehaviour
{
    [SerializeField] private GameObject cannonballPrefab;

    private void OnTriggerEnter(Collider other)
    {
        InteractionEvent interactionEvent = other.GetComponent<InteractionEvent>();
        if (interactionEvent != null)
        {
            if (interactionEvent.GetAction() != InteractionEvent.Action.Interact) return;

            if (interactionEvent.GetPC().GetHeldObject() == null)
            {
                GameObject cannonball = Instantiate(cannonballPrefab);
                cannonball.GetComponent<HoldableObject>().SetHolder(interactionEvent.GetPC(), new Vector3(0f, 0.9f, -0f));
            }
            else if (interactionEvent.GetPC().GetHeldObject().name == "Cannonball Item(Clone)")
            {
                Destroy(interactionEvent.GetPC().GetHeldObject().gameObject);
            }
        }
    }
}