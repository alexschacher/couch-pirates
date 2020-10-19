using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BucketEmptyZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        InteractionEvent interactionEvent = other.GetComponent<InteractionEvent>();
        if (interactionEvent != null)
        {
            if (interactionEvent.GetAction() != InteractionEvent.Action.Interact) return;

            if (interactionEvent.GetPC().GetHeldObject() == null)
            {
                BucketController bucket = interactionEvent.GetPC().GetHeldObject().GetComponent<BucketController>();
                if (bucket != null)
                {
                    if (bucket.HasWater())
                    {
                        bucket.SetHasWater(false);
                    }
                }
            }
        }
    }
}
