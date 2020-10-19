using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ocean : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Cannonball cannonball = other.gameObject.GetComponent<Cannonball>();
        if (cannonball != null)
        {
            cannonball.Splash();
            return;
        }
    }
}