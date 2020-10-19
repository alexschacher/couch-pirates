using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldableObject : MonoBehaviour
{
    private PlayerController playerController;
    private bool isHeld = false;

    public void SetHolder(PlayerController pc, Vector3 offset)
    {
        playerController = pc;
        transform.SetParent(pc.transform);
        transform.position = pc.transform.position;
        transform.position += offset;
        pc.SetHeldObject(this);
        isHeld = true;
    }

    public void DropOnGround()
    {
        transform.parent = null;
        isHeld = false;
        if (playerController != null)
        {
            transform.position = playerController.transform.position;
            playerController.RemoveHeldObject();
            playerController = null;
        }
    }

    public void OnDestroy()
    {
        if (playerController != null)
        {
            playerController.RemoveHeldObject();
        }
    }

    public bool IsHeld()
    {
        return isHeld;
    }
}