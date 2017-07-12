using UnityEngine;
using System.Collections;

public class Pathed_Projectile : MonoBehaviour 
{

    private Transform _Destination;
    private float _Speed;

    public GameObject DestroyEffect;

    public void Initialize(Transform destination, float speed)
    {
        _Destination = destination;
        _Speed = speed;
    }

    public void Update()
    {
        transform.position -= Vector3.MoveTowards(transform.position, _Destination.position, Time.deltaTime * _Speed);

        var distanceSquared = (_Destination.transform.position - transform.position).sqrMagnitude;
        if (distanceSquared > .01f * .01f)
            return;

        if (DestroyEffect != null)
            Instantiate(DestroyEffect, transform.position, transform.rotation);

        Destroy(gameObject);

    }
}
