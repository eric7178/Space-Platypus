using UnityEngine;
using System.Collections;

public class Point_Item : MonoBehaviour, IPlayer_Respawn_Listener
{
    public GameObject Effect;
    public int points;

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Player>() == null)
            return;

        Game_Manager.Instance.AddPoints(points);
        Instantiate(Effect, transform.position, transform.rotation);
        gameObject.SetActive(false);

        Floating_Text.Show(string.Format("+{0}!", points), "Item Text", new From_WorldPoint_Text_Positioner(Camera.main, transform.position, 1.5f, 50f));

    }

    public void OnPlayerRespawn(CheckPoint point, Player p)
    {
        gameObject.SetActive(true);
    }
	
}
