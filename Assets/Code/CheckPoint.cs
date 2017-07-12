using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CheckPoint : MonoBehaviour 
{
    private List<IPlayer_Respawn_Listener> _listeners;

    public void Awake()
    {
        _listeners = new List<IPlayer_Respawn_Listener>();

    }

    public void PlayerHitChechpoint()
    {
        StartCoroutine(PlayerHitChechpointCo(Level_Manager.Instance.CurrentTimeBonus));
    }

    private IEnumerator PlayerHitChechpointCo(int Bonus)
    {
        Floating_Text.Show("CheckPoint","CheckPointText", new Centered_Text_Positioner(.5f));
        yield return new WaitForSeconds(.5f);
        Floating_Text.Show(string.Format("+{0}! Time Bonus!", Bonus), "Check Point Text", new Centered_Text_Positioner(.5f));

    }

    public void PlayerLeftCheckpoint()
    {

    }

    public void SpawnPlayer(Player player)
    {
        player.RespawnAt(transform);

        foreach (var listener in _listeners)
            listener.OnPlayerRespawn(this, player);
            
    }

    public void AssignObjectToChackpoint(IPlayer_Respawn_Listener listener)
    {
        _listeners.Add(listener);
    }


}
