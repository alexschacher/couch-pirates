using UnityEngine;

public class InteractionEvent : MonoBehaviour
{
    public enum Action { Interact, Grab, Drop}

    private PlayerController playerController;
    private Action action;

    public void Init(PlayerController pc, Action action)
    {
        this.playerController = pc;
        this.action = action;
        Destroy(this.gameObject, 0.1f);
    }

    public PlayerController GetPC() => playerController;
    public Action GetAction() => action;
}