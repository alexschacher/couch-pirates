using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] GameObject p1;
    [SerializeField] GameObject p2;
    [SerializeField] GameObject p3;
    [SerializeField] GameObject p4;

    private void Start()
    {
        p1.SetActive(false);
        p2.SetActive(false);
        p3.SetActive(false);
        p4.SetActive(false);
    }

    void Update()
    {
        ActivatePlayers();
    }

    private void ActivatePlayers()
    {
        if (Input.GetButtonDown("Button_Bottom_P1"))
        {
            p1.SetActive(true);
        }
        if (Input.GetButtonDown("Button_Bottom_P2"))
        {
            p2.SetActive(true);
        }
        if (Input.GetButtonDown("Button_Bottom_P3"))
        {
            p3.SetActive(true);
        }
        if (Input.GetButtonDown("Button_Bottom_P4"))
        {
            p4.SetActive(true);
        }
    }
}
