using UnityEngine;


public class Game_Manager 
{
    private static Game_Manager _instance;
    public static Game_Manager Instance { get { return _instance ?? (_instance = new Game_Manager()); } }
    public int Points { get; set; }

    private Game_Manager()
    {

    }

    public void Reset()
    {
        Points = 0;
    }

    public void ResetPoints(int p)
    {
        Points = p;
    }

    public void AddPoints(int p)
    {
        Points += p;
    }


}
