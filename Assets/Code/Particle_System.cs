using UnityEngine;
using System.Collections;

public class Particle_System : MonoBehaviour 
{

    public string LayerName = "ParticleLayer";

    public void Start()
    {
        GetComponent<ParticleSystem>().GetComponent<Renderer>().sortingLayerName = LayerName;
    }
}
