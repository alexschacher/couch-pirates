using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BucketController : MonoBehaviour
{
    private int playerID;
    private bool hasWater = false;
    [SerializeField] private GameObject bucketWater;
    private HoldableObject holdableObject;

    private void Start()
    {
        holdableObject = GetComponent<HoldableObject>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (holdableObject.IsHeld()) return;

        InteractionEvent interactionEvent = other.GetComponent<InteractionEvent>();
        if (interactionEvent != null)
        {
            if (interactionEvent.GetAction() != InteractionEvent.Action.Grab) return;

            if (interactionEvent.GetPC().GetHeldObject() == null)
            {
                holdableObject.SetHolder(interactionEvent.GetPC(), new Vector3(0f, 0.75f, -0f));
                playerID = interactionEvent.GetPC().GetPlayerId();
            }
        }
    }

    void Update()
    {
        if (!holdableObject.IsHeld()) return;

        if (Input.GetButtonDown("Button_Top_P" + playerID))
        {
            holdableObject.DropOnGround();
        }
    }

    public bool HasWater()
    {
        return hasWater;
    }

    public void SetHasWater(bool hasWater)
    {
        this.hasWater = hasWater;
        bucketWater.SetActive(hasWater);
    }

    
}
