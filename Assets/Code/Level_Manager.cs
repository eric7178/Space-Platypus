using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Level_Manager : MonoBehaviour 
{

    public static Level_Manager Instance { get; private set; }

    public Player tPlayer { get; private set; }
    public Camera_Controller Camera { get; private set; }
    public TimeSpan RunningTime { get { return DateTime.UtcNow - _Started; } }
    public int CurrentTimeBonus
    {
        get
        {
            var SecDiff = (int)(BonusCutOffSec - RunningTime.TotalSeconds);
            return Mathf.Max(0, SecDiff) * BonusSecMultiply;
        }
    }

    private List<CheckPoint> _checkPoints;
    private int _CurrentCheckpointIndex;
    private DateTime _Started;
    private int _SavePoints;

    public CheckPoint DebugSpawn;
    public int BonusCutOffSec;
    public int BonusSecMultiply;


    public void Awake()
    {
        Instance = this;
    }

    public void Start()
    {
        _checkPoints = FindObjectsOfType<CheckPoint>().OrderBy(t => t.transform.position.x).ToList();
        _CurrentCheckpointIndex = _checkPoints.Count > 0 ? 0 : -1;

        tPlayer = FindObjectOfType<Player>();
        Camera = FindObjectOfType<Camera_Controller>();

        _Started = DateTime.UtcNow;

        var listeners = FindObjectsOfType<MonoBehaviour>().OfType<IPlayer_Respawn_Listener>();

        foreach (var listener in listeners)
        {
            for (var j = _checkPoints.Count - 1; j >= 0; j--)
            {
                var distance = ((MonoBehaviour)listener).transform.position.x - _checkPoints[j].transform.position.x;
                if (distance < 0)
                    continue;

                _checkPoints[j].AssignObjectToChackpoint(listener);
                break;

            }
        }

#if UNITY_EDITOR
        if (DebugSpawn != null)
            DebugSpawn.SpawnPlayer(tPlayer);
        else if (_CurrentCheckpointIndex != -1)
            _checkPoints[_CurrentCheckpointIndex].SpawnPlayer(tPlayer);
#else
        if(_CurrentCheckpointIndex != -1)
            _CheckPoints[_CurrentCheckpointIndex].SpawnPlayer(Player);
#endif

    }

    public void Update()
    {
        var isLastCheckpoint = _CurrentCheckpointIndex + 1 >= _checkPoints.Count;
        if (isLastCheckpoint)
            return;
        var distanceToNextCheckPoint = _checkPoints[_CurrentCheckpointIndex + 1].transform.position.x - tPlayer.transform.position.x;
        if (distanceToNextCheckPoint >= 0)
            return;

        _checkPoints[_CurrentCheckpointIndex].PlayerLeftCheckpoint();
        _CurrentCheckpointIndex++;
        _checkPoints[_CurrentCheckpointIndex].PlayerHitChechpoint();

        Game_Manager.Instance.AddPoints(CurrentTimeBonus);
        _SavePoints = Game_Manager.Instance.Points;
        _Started = DateTime.UtcNow;

        //make more efficient
       
    }

    public void KillPlayer()
    {
        StartCoroutine(KillPlayerCo());
    }

    private IEnumerator KillPlayerCo()
    {
        tPlayer.Kill();
        Camera.IsFollowing = false;
        yield return new WaitForSeconds(2f);

        Camera.IsFollowing = true;
        if (_CurrentCheckpointIndex != -1)
            _checkPoints[_CurrentCheckpointIndex].SpawnPlayer(tPlayer);

       // TODO: POINTS
        _Started = DateTime.UtcNow;
        Game_Manager.Instance.ResetPoints(_SavePoints);

    }

}
