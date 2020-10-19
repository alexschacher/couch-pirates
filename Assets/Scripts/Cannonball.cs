using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannonball : MonoBehaviour
{
    [SerializeField] private float speed;
    private Vector3 parabolaStart;
    private Vector3 parabolaEnd;
    private float parabolaHeight;
    private float progressAlongTrajectory;
    [SerializeField] private GameObject cannonballCrash;
    [SerializeField] private GameObject cannonballSplash;

    public void Init(Vector3 parabolaStart, Vector3 parabolaEnd, float parabolaHeight, bool isPlayerShot)
    {
        this.parabolaStart = parabolaStart;
        this.parabolaEnd = parabolaEnd;
        this.parabolaHeight = parabolaHeight;
        if (isPlayerShot)
        {
            gameObject.layer = LayerMask.NameToLayer("EnemyHitbox");
        }
        else
        {
            gameObject.layer = LayerMask.NameToLayer("PlayerShipHitbox");
        }
    }

    void Update()
    {
        progressAlongTrajectory += Time.deltaTime * speed;
        transform.position = MathParabola.Parabola(parabolaStart, parabolaEnd, parabolaHeight, progressAlongTrajectory);
    }

    public void Crash()
    {
        Instantiate(cannonballCrash, transform.position, transform.rotation);
        Destroy(this.gameObject);
    }

    public void Splash()
    {
        Instantiate(cannonballSplash, transform.position, cannonballSplash.transform.rotation);
        Destroy(this.gameObject);
    }
}