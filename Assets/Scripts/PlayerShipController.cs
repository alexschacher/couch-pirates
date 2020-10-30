using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShipController : MonoBehaviour
{
    [SerializeField] private List<FixSpot> leftFixHoles;
    [SerializeField] private List<FixSpot> rightFixHoles;

    private void OnTriggerEnter(Collider other)
    {
        Cannonball cannonball = other.gameObject.GetComponent<Cannonball>();
        if (cannonball != null)
        {
            cannonball.Crash();

            if (cannonball.CameFromLeft())
            {
                int randomIndex = Random.Range(0, leftFixHoles.Count);
                leftFixHoles[randomIndex].TakeDamage();
            }
            else
            {
                int randomIndex = Random.Range(0, rightFixHoles.Count);
                rightFixHoles[randomIndex].TakeDamage();
            }
        }
    }
}