using UnityEngine;
using System.Collections;

public class Game_HUD : MonoBehaviour 
{
    public GUISkin gSkin;

    public void OnGUI()
    {
        GUI.skin = gSkin;
        GUILayout.BeginArea(new Rect(0, 0, Screen.width, Screen.height));
        //begin
        GUILayout.BeginVertical(gSkin.GetStyle("GameHUD"));
        GUILayout.Label(string.Format("Points: {0}", Game_Manager.Instance.Points), gSkin.GetStyle("pointsText"));
        var time = Level_Manager.Instance.RunningTime;
        GUILayout.Label(string.Format("{0:00}: {1:00} with {2} bonus", time.Minutes + (time.Hours * 60), time.Seconds, Level_Manager.Instance.CurrentTimeBonus), gSkin.GetStyle("timeText"));
        //end
        GUILayout.EndVertical();
        GUILayout.EndArea();
    }


}
