using UnityEngine;
using System.Collections;

public class InstaKill : MonoBehaviour 
{
    public void OnTriggerEnter(Collider other)
    {
        var player = other.GetComponent<Player>();
        if (player == null)
            return;

        Level_Manager.Instance.KillPlayer();

    }
}
