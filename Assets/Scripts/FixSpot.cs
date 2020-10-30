using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixSpot : MonoBehaviour
{
    public enum Plank { Top, Mid, Bottom }
    public enum State { Broken, Plank, OneNail, Fixed }

    private State topState = State.Fixed;
    private State midState = State.Fixed;
    private State bottomState = State.Fixed;

    [InspectorButton("TakeDamage")] [SerializeField] private bool takeDamageButton;
    [InspectorButton("Fix")] [SerializeField] private bool fixButton;
    [InspectorButton("StopFixing")] [SerializeField] private bool stopFixingButton;

    [SerializeField] private float stopFixingTime = 0.5f;
    private float fixTimer;
    public bool canTakeDamage = true;

    [SerializeField] private GameObject plankUp;
    [SerializeField] private GameObject plankMid;
    [SerializeField] private GameObject plankBottom;

    [SerializeField] private GameObject nailUpLeft;
    [SerializeField] private GameObject nailMidLeft;
    [SerializeField] private GameObject nailBottomLeft;

    [SerializeField] private GameObject nailUpRight;
    [SerializeField] private GameObject nailMidRight;
    [SerializeField] private GameObject nailBottomRight;

    [SerializeField] private GameObject streamUp;
    [SerializeField] private GameObject streamMid;
    [SerializeField] private GameObject streamBottom;

    private void Start()
    {
        streamUp.SetActive(false);
        streamMid.SetActive(false);
        streamBottom.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        InteractionEvent interaction = other.GetComponent<InteractionEvent>();
        if (interaction == null) return;
        if (interaction.GetAction() != InteractionEvent.Action.Interact) return;
        Fix();
        fixTimer = stopFixingTime;
    }

    private void Update()
    {
        if (fixTimer > 0)
        {
            fixTimer -= Time.deltaTime;
        }
        else if (fixTimer > -100)
        {
            fixTimer = -101f;
            StopFixing();
        }
    }

    public void TakeDamage()
    {
        if (!canTakeDamage) return;

        if (midState == State.Fixed)
        {
            SetPlankState(Plank.Mid, State.Broken);
        }
        else if (bottomState == State.Fixed)
        {
            SetPlankState(Plank.Bottom, State.Broken);
        }
        else if (topState == State.Fixed)
        {
            SetPlankState(Plank.Top, State.Broken);
        }
    }

    private void Fix()
    {
        if (midState != State.Fixed)
        {
            SetPlankState(Plank.Mid, (State)((int)midState + 1));
        }
        else if (bottomState != State.Fixed)
        {
            SetPlankState(Plank.Bottom, (State)((int)bottomState + 1));
        }
        else if(topState != State.Fixed)
        {
            SetPlankState(Plank.Top, (State)((int)topState + 1));
        }
    }

    private void StopFixing()
    {
        if (midState != State.Fixed)
        {
            SetPlankState(Plank.Mid, State.Broken);
        }
        if (bottomState != State.Fixed)
        {
            SetPlankState(Plank.Bottom, State.Broken);
        }
        if (topState != State.Fixed)
        {
            SetPlankState(Plank.Top, State.Broken);
        }
    }

    private void SetPlankState(Plank plank, State state)
    {
        switch (plank)
        {
            case Plank.Mid: midState = state; break;
            case Plank.Top: topState = state; break;
            case Plank.Bottom: bottomState = state; break;
        }

        switch (state)
        {
            case State.Broken: SetPlank_Broken(plank); break;
            case State.Plank: SetPlank_Plank(plank); break;
            case State.OneNail: SetPlank_OneNail(plank); break;
            case State.Fixed: SetPlank_Fixed(plank); break;
            default: break;
        }
    }

    private void SetPlank_Broken(Plank plank)
    {
        switch (plank)
        {
            case Plank.Top:
                {
                    streamUp.SetActive(true);
                    plankUp.SetActive(false);
                    nailUpLeft.SetActive(false);
                    nailUpRight.SetActive(false);
                }
                break;
            case Plank.Mid:
                {
                    streamMid.SetActive(true);
                    plankMid.SetActive(false);
                    nailMidLeft.SetActive(false);
                    nailMidRight.SetActive(false);
                }
                break;
            case Plank.Bottom:
                {
                    streamBottom.SetActive(true);
                    plankBottom.SetActive(false);
                    nailBottomLeft.SetActive(false);
                    nailBottomRight.SetActive(false);
                }
                break;
            default: break;
        }
    }

    private void SetPlank_Plank(Plank plank)
    {
        switch (plank)
        {
            case Plank.Top:
                {
                    streamUp.SetActive(false);
                    plankUp.SetActive(true);
                    nailUpLeft.SetActive(false);
                    nailUpRight.SetActive(false);
                }
                break;
            case Plank.Mid:
                {
                    streamMid.SetActive(false);
                    plankMid.SetActive(true);
                    nailMidLeft.SetActive(false);
                    nailMidRight.SetActive(false);
                }
                break;
            case Plank.Bottom:
                {
                    streamBottom.SetActive(false);
                    plankBottom.SetActive(true);
                    nailBottomLeft.SetActive(false);
                    nailBottomRight.SetActive(false);
                }
                break;
            default: break;
        }
    }

    private void SetPlank_OneNail(Plank plank)
    {
        switch (plank)
        {
            case Plank.Top:
                {
                    streamUp.SetActive(false);
                    plankUp.SetActive(true);
                    nailUpLeft.SetActive(true);
                    nailUpRight.SetActive(false);
                }
                break;
            case Plank.Mid:
                {
                    streamMid.SetActive(false);
                    plankMid.SetActive(true);
                    nailMidLeft.SetActive(true);
                    nailMidRight.SetActive(false);
                }
                break;
            case Plank.Bottom:
                {
                    streamBottom.SetActive(false);
                    plankBottom.SetActive(true);
                    nailBottomLeft.SetActive(true);
                    nailBottomRight.SetActive(false);
                }
                break;
            default: break;
        }
    }

    private void SetPlank_Fixed(Plank plank)
    {
        switch (plank)
        {
            case Plank.Top:
                {
                    streamUp.SetActive(false);
                    plankUp.SetActive(true);
                    nailUpLeft.SetActive(true);
                    nailUpRight.SetActive(true);
                }
                break;
            case Plank.Mid:
                {
                    streamMid.SetActive(false);
                    plankMid.SetActive(true);
                    nailMidLeft.SetActive(true);
                    nailMidRight.SetActive(true);
                }
                break;
            case Plank.Bottom:
                {
                    streamBottom.SetActive(false);
                    plankBottom.SetActive(true);
                    nailBottomLeft.SetActive(true);
                    nailBottomRight.SetActive(true);
                }
                break;
            default: break;
        }
    }

    public void SetAll(State state)
    {
        SetPlankState(Plank.Top, state);
        SetPlankState(Plank.Mid, state);
        SetPlankState(Plank.Bottom, state);
    }
}
