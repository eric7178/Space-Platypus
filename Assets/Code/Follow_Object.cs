using UnityEngine;
using System.Collections;

public class Follow_Object : MonoBehaviour 
{
    public Vector2 offset;
    public Transform Following;

    public void Update()
    {
        transform.position = Following.transform.position + (Vector3)offset;

    }
}
