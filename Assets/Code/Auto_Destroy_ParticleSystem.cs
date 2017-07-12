using UnityEngine;
using System.Collections;

public class Auto_Destroy_ParticleSystem : MonoBehaviour 
{
    private ParticleSystem _ParticleSystem;

    public void Start()
    {
        _ParticleSystem = GetComponent<ParticleSystem>();
    }

    public void Update()
    {
        if (_ParticleSystem.isPlaying)
            return;

        Destroy(gameObject);

    }
}
