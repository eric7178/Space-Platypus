using UnityEngine;
using System.Collections;

public class Camera_Controller : MonoBehaviour 
{

    public GameObject PlayerFocus;
    public Vector2 Margin;
    public Vector2 Smoothing;
    public BoxCollider2D Bounds; //2d Sucks
    private Vector3 _min;
    private Vector3 _max;

    public bool IsFollowing { get; set; }
    /// <summary>
    /// ///////////////////////////////

    public float distance = 20.0f;
    public float height = 5.0f;
    public float heightDamping = 2.0f;

    public float lookAtHeight = 0.0f;

    public Rigidbody parentRigidbody;

    public float rotationSnapTime = 0.3F;

    public float distanceSnapTime;
    public float distanceMultiplier;

    private Vector3 lookAtVector;

    private float usedDistance;

    float wantedRotationAngle;
    float wantedHeight;

    float currentRotationAngle;
    float currentHeight;

    Quaternion currentRotation;
    Vector3 wantedPosition;

    private float yVelocity = 0.0F;
    private float zVelocity = 0.0F;
    /// </summary>
    /// 

    public void Start()
    {
        _min = Bounds.bounds.min;
        _max = Bounds.bounds.max;
        IsFollowing = true;

        lookAtVector = new Vector3(0, lookAtHeight, 0);
        PlayerFocus = GetComponent<GameObject>();
    }

    public void Update()
    {
        var x = transform.position.x;
        var y = transform.position.y;

        if (IsFollowing)
        {
            if (Mathf.Abs(x - PlayerFocus.transform.position.x) > Margin.x)
                x = Mathf.Lerp(x, PlayerFocus.transform.position.x, Smoothing.x * Time.deltaTime);

            if (Mathf.Abs(y - PlayerFocus.transform.position.y) > Margin.y)
                y = Mathf.Lerp(y, PlayerFocus.transform.position.y, Smoothing.y * Time.deltaTime);
        }
        var cameraHalfWidth = GetComponent<Camera>().orthographicSize * ((float)Screen.width / Screen.height);
        x = Mathf.Clamp(x, _min.x + cameraHalfWidth, _max.x - cameraHalfWidth);
        y = Mathf.Clamp(x, _min.y + cameraHalfWidth, _max.y - cameraHalfWidth);
        y = Mathf.Clamp(y, _min.y + GetComponent<Camera>().orthographicSize, _max.y - GetComponent<Camera>().orthographicSize);

        transform.position = new Vector3(x, y, transform.position.z);


        //transform.RotateAround(Player.position, Vector3.up, 10 * Time.deltaTime);


    }
    void LateUpdate()
    {

        wantedHeight = PlayerFocus.transform.position.y + height;
        currentHeight = transform.position.y;

        wantedRotationAngle = PlayerFocus.transform.eulerAngles.y;
        currentRotationAngle = transform.transform.eulerAngles.y;

        currentRotationAngle = Mathf.SmoothDampAngle(currentRotationAngle, wantedRotationAngle, ref yVelocity, rotationSnapTime);

        currentHeight = Mathf.Lerp(currentHeight, wantedHeight, heightDamping * Time.deltaTime);

        wantedPosition = PlayerFocus.transform.position;
        wantedPosition.y = currentHeight;

        usedDistance = Mathf.SmoothDampAngle(usedDistance, distance + (PlayerFocus.GetComponentInParent<Rigidbody>().velocity.magnitude * distanceMultiplier), ref zVelocity, distanceSnapTime);

        wantedPosition += Quaternion.Euler(0, currentRotationAngle, 0) * new Vector3(0, 0, -usedDistance);

        transform.position = wantedPosition;

        transform.LookAt(PlayerFocus.transform.position + lookAtVector);

    }
	
}
