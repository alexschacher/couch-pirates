using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvisibilityTriggerZone : MonoBehaviour
{
    [SerializeField] private List<GameObject> objectsToHide;
    private List<PlayerController> collidingPlayers;

    private void Start()
    {
        collidingPlayers = new List<PlayerController>();
        SetActivityOfAll(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerController pc = other.GetComponent<PlayerController>();
        if (pc != null)
        {
            collidingPlayers.Add(pc);
            SetActivityOfAll(false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        PlayerController pc = other.GetComponent<PlayerController>();
        if (pc != null)
        {
            if (collidingPlayers.Contains(pc))
            {
                collidingPlayers.Remove(pc);

                if (collidingPlayers.Count == 0)
                {
                    SetActivityOfAll(true);
                }
            }
        }
    }

    private void SetActivityOfAll(bool toSet)
    {
        foreach(GameObject obj in objectsToHide)
        {
            obj.SetActive(toSet);
        }
    }
}
