using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlankCollision : MonoBehaviour
{
    private Animator animator;
    private int state;
    private float delayTimer;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (delayTimer >= 0)
        {
            delayTimer -= Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Cannonball>() != null)
        {
            if (delayTimer <= 0)
            {
                delayTimer = 0.2f;

                state++;
                if (state > 3)
                {
                    state = 3;
                }

                animator.Play("PlankState" + state);
            }
        }
    }
}