using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Follow_Path : MonoBehaviour 
{
    public enum followType { moveTowards, Lerp }

    public followType fType = followType.moveTowards;
    public Path_Definition Path;

    public float speed = 1f;
    public float maxDistancetoGoal = .2f;

    private IEnumerator<Transform> _currWayPoint;

    public void Start()
    {
        if (Path == null)
        {
            Debug.LogError("Path cannot be null!!", gameObject);
            return;
        }

        _currWayPoint = Path.GetPathEnumerator();
        _currWayPoint.MoveNext();

        if (_currWayPoint.Current == null)
            return;

        transform.position = _currWayPoint.Current.position;
    }

    public void Update()
    {
        if (_currWayPoint == null || _currWayPoint.Current == null)
            return;

        if (fType == followType.moveTowards)
            transform.position = Vector3.MoveTowards(transform.position, _currWayPoint.Current.position, Time.deltaTime * speed);
        else if (fType == followType.Lerp)
            transform.position = Vector3.Lerp(transform.position, _currWayPoint.Current.position, Time.deltaTime * speed);

        var distanceSquared = (transform.position - _currWayPoint.Current.position).sqrMagnitude;
        if (distanceSquared < maxDistancetoGoal * maxDistancetoGoal)
            _currWayPoint.MoveNext();

    }
	
}
