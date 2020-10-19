using UnityEngine;

public class StationController : MonoBehaviour
{
    [SerializeField] private GameObject stationPosition;
    private PlayerController playerController;

    public void TakeControlFromPlayer(PlayerController pc)
    {
        playerController = pc;
        playerController.gameObject.transform.position = stationPosition.transform.position;
        playerController.gameObject.transform.rotation = stationPosition.transform.rotation;
        playerController.SetHasControl(false);
    }

    public void ReturnControlToPlayer()
    {
        playerController.SetHasControl(true);
    }
}