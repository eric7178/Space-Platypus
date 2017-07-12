using UnityEngine;
using System.Collections;

public class Health_Bar : MonoBehaviour 
{
    public Player tPlayer;
    public Transform ForegroundSprite;
    public SpriteRenderer foregroundRenderer;
    public Color MaxHealthColor = new Color(255/255f,63/255f,63/255f);
    public Color MinHealthColor = new Color(64 / 255f, 137 / 255f, 255 / 255f);

    public void Update()
    {
        var healthpercent = tPlayer.Health / (float)tPlayer.maxhealth;
        ForegroundSprite.localScale = new Vector3(healthpercent,1,1);
        foregroundRenderer.color = Color.Lerp(MaxHealthColor, MinHealthColor, healthpercent);

    }



}
