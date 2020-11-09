using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandHazard : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        EnemyShipController enemyShipController = other.GetComponent<EnemyShipController>();
        if (enemyShipController != null)
        {
            CollideWithEnemyShip(enemyShipController);
        }
    }

    private void CollideWithEnemyShip(EnemyShipController enemyShipController)
    {
        enemyShipController.HitIsland();
    }
}
