using UnityEngine;
using System.Collections;
//add to camera
public class Background_Parallax : MonoBehaviour {

    public Transform[] Backgrounds;

    public float parallaxScale;
    public float parallaxReductionFactor;
    public float smoothing;

    private Vector3 _LastPosition;

    public void Start()
    {
        _LastPosition = transform.position;
    }

    public void Update()
    {
        var parallax = (_LastPosition.x - transform.position.x) * parallaxScale;
        for (var i = 0; i < Backgrounds.Length; i++)
        {
            var backgroundTargetPos = Backgrounds[i].position.x + parallax * (i * parallaxReductionFactor + 1);
            Backgrounds[i].position = Vector3.Lerp(Backgrounds[i].position,new Vector3(backgroundTargetPos ,Backgrounds[i].position.y, Backgrounds[i].position.z),smoothing * Time.deltaTime);

        }
        _LastPosition = transform.position;
    }

}
