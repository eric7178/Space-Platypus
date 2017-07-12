using UnityEngine;
using System.Collections;

public class Pathed_Projectile_Spawner : MonoBehaviour 
{
    public Transform Destination;
    public Pathed_Projectile Projectile;

    public GameObject SpawnEffect;

    public float Speed;
    public float FireRate;

    public float _NextShotInSeconds;


    public void Start()
    {
        _NextShotInSeconds = FireRate;

    }

    public void Update()
    {
        if ((_NextShotInSeconds -= Time.deltaTime) > 0)
            return;

        _NextShotInSeconds = FireRate;

        var projectile = (Pathed_Projectile)Instantiate(Projectile, transform.position, transform.rotation);
        Projectile.Initialize(Destination, Speed);
        if (SpawnEffect != null)
            Instantiate(SpawnEffect, transform.position, transform.rotation);

    }

    public void OnDrawGizmos()
    {
        if (Destination == null)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, Destination.position);

    }

}
