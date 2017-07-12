using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Path_Definition : MonoBehaviour 
{
    public Transform[] wayPoints;


    public IEnumerator<Transform> GetPathEnumerator()
    {
       

        if (wayPoints == null || wayPoints.Length < 2)
            yield break;

        int direction = 1;
        int index = 0;
        while (true)
        {
            yield return wayPoints[index];

            if (wayPoints.Length == 1)
                continue;

            if (index <= 0)
                direction = 1;
            else if (index >= wayPoints.Length - 1)
                direction = -1;

            index += direction;



        }


    }

    public void OnDrawGizmos()
    {
        if (wayPoints == null || wayPoints.Length < 2)
            return;

        var points = wayPoints.Where(t => t != null).ToList();
        if (points.Count < 2)
            return;

        for (int i = 1; i < points.Count; i++)
        {
            Gizmos.DrawLine(points[i - 1].position, points[i].position);

        }
    }

    public void OnDrawGizmosSelected()
    {

    }
}
