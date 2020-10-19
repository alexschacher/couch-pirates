using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelmController : MonoBehaviour
{
    [SerializeField] private GameObject wheel;
    [SerializeField] private float wheelSpinSpeed;
    private StationController stationController;
    private int playerID;
    private bool isInUse = false;

    private void Start()
    {
        stationController = GetComponent<StationController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isInUse) return;

        InteractionEvent interactionEvent = other.GetComponent<InteractionEvent>();
        if (interactionEvent != null)
        {
            if (interactionEvent.GetAction() != InteractionEvent.Action.Interact) return;

            if (interactionEvent.GetPC().GetHeldObject() == null)
            {
                playerID = interactionEvent.GetPC().GetPlayerId();
                isInUse = true;
                stationController.TakeControlFromPlayer(interactionEvent.GetPC());
            }
        }
    }

    private void Update()
    {
        if (!isInUse) return;

        float horizontal = Input.GetAxisRaw("Walk_H_P" + playerID);
        wheel.transform.Rotate(new Vector3(0f, 0f, -horizontal * wheelSpinSpeed));

        if (!Input.GetButton("Button_Left_P" + playerID) && !Input.GetButton("Button_Bottom_P" + playerID))
        {
            stationController.ReturnControlToPlayer();
            isInUse = false;
        }
    }
}