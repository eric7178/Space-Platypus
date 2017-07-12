using UnityEngine;
using System.Collections;

public class Character_Controller_2D : MonoBehaviour 
{
    private const float skinWidth = .0002f;
    private const int totalHorizontalRays = 8;
    private const int totalVerticalRays = 4;
    private static readonly float slopeLimitTangant = Mathf.Tan(75f * Mathf.Deg2Rad);

    public LayerMask PlatformMask;
    public Controller_Parameters_2D DefaultParameters;
    
    public Controller_State_2D State { get; private set; }

    public Vector3 Velocity { get { return _Velocity; } }

    public Vector3 platformVelocity {get; private set;}

  

    public bool canJump 
    {
        get 
        {
            if (Parameters.jumpRestrictions == Controller_Parameters_2D.jumpBehavior.canJumpAnywhere)
                return _JumpIn <= 0;

            if (Parameters.jumpRestrictions == Controller_Parameters_2D.jumpBehavior.canJumpGround)
                return State.isGrounded;

            return false;
        } 
    }

    public bool HandleCollisions { get; set; }
    public Controller_Parameters_2D Parameters { get { return _OverrideParameters ?? DefaultParameters; } }
    public GameObject standingOn { get; private set; }

    private Vector3 _Velocity;
    private Transform _Transform;
    private Vector3 _LocalScale;
    private BoxCollider _boxCollider;
    private Controller_Parameters_2D _OverrideParameters;

    private float _VerticalRayDistance;
    private float _HorizontalRayDistance;
    private Vector3 _RaycastTopLeft;
    private Vector3 _RaycastBottomRight;
    private Vector3 _RaycastBottomLeft;
    private float _JumpIn;
    private GameObject _LastStandingOn;

    private Vector3 _ActiveGlobalPlatformPoint;
    private Vector3 _ActiveLocalPlatformPoint;
    private RaycastHit HitInfo;
   

   

    public void Awake()
    {
        

        HandleCollisions = true;

        State = new Controller_State_2D();
        _Transform = transform;
        _LocalScale = transform.localScale;
        _boxCollider = GetComponent<BoxCollider>();
       
        var colliderWidth = _boxCollider.size.x * Mathf.Abs(transform.localScale.x) - (2 * skinWidth);
        _HorizontalRayDistance = colliderWidth / (totalVerticalRays - 1);
            
        var colliderHeight = _boxCollider.size.y * Mathf.Abs(transform.localScale.y) - (2 * skinWidth);
        _VerticalRayDistance = colliderHeight / (totalHorizontalRays - 1);
       
    }

   

 

    public void LateUpdate()
    {
       
        _JumpIn -= Time.deltaTime;

        _Velocity.y += Parameters.gravity * Time.deltaTime;
       
       
   
        Move(Velocity * Time.deltaTime);
       


    }

    public void OnTriggerEnter(Collider other)
    {
        var parameters = other.gameObject.GetComponent<Controller_Physics_Volume_2D>();
       
        if (parameters == null)
            return;

        _OverrideParameters = parameters.Parameters;
    }

    public void OnTriggerStay(Collider other)
    {

        var parameters = other.gameObject.GetComponent<Controller_Physics_Volume_2D>();
        if (parameters == null)
            return;

        _OverrideParameters = parameters.Parameters;
    }

    public void OnTriggerExit(Collider other)
    {

        var parameters = other.gameObject.GetComponent<Controller_Physics_Volume_2D>();
        if (parameters == null)
            return;

        _OverrideParameters = null;
    }

    public void AddForce(Vector3 force)
    {
        _Velocity = force;
    }

    public void SetForce(Vector3 force)
    {
        _Velocity += force;
    }

    public void SetHorizontalForce(float x)
    {
        _Velocity.x = x;
    }

    public void SetVerticalForce(float y)
    {
        _Velocity.y = y;
       
    }

    public void Jump()
    {//dont forget to handle moving platforms
       
        AddForce(new Vector2(0, Parameters.jumpMagnitude));
       
        //transform.Translate(0, Parameters.jumpMagnitude * Time.deltaTime, 0);

       


        _JumpIn = Parameters.jumpFrequentcy;

        
    }

    private void Move(Vector3 DeltaMovement)
    {
        var wasGrounded = State.isCollidingBelow;
        State.Reset();

        if (HandleCollisions)
        {
            HandlePlatforms();
            CalculateRayOrigins();

            if (DeltaMovement.y < 0 && wasGrounded)
                HandleVerticalSlope(ref DeltaMovement);

            if (Mathf.Abs(DeltaMovement.x) > .001f)
                MoveHorizontially(ref DeltaMovement);

            MoveVertically(ref DeltaMovement);
            CorrectHorizontalPlacement(ref DeltaMovement, true);
            CorrectHorizontalPlacement(ref DeltaMovement, false);

        }

      
       

        if (Time.deltaTime > 0)
            _Velocity = DeltaMovement / Time.deltaTime;

        _Velocity.x = Mathf.Min(_Velocity.x, Parameters.maxVelocity.x);
        _Velocity.y = Mathf.Min(_Velocity.y, Parameters.maxVelocity.y);

        //if (State.isMovingUpSlope)
            //_Velocity.y = 0;

        if (standingOn != null)
        {
            _ActiveGlobalPlatformPoint = transform.position;
            _ActiveLocalPlatformPoint = standingOn.transform.InverseTransformPoint(transform.position);
            if (_LastStandingOn != standingOn)
            {
                if (_LastStandingOn != null)
                    _LastStandingOn.SendMessage("ControllerExit",this,SendMessageOptions.DontRequireReceiver);

                standingOn.SendMessage("ControllerEnter", this, SendMessageOptions.DontRequireReceiver);
                _LastStandingOn = standingOn;
                    
            }
            else if(standingOn != null)
                standingOn.SendMessage("ControllerStay", this, SendMessageOptions.DontRequireReceiver);
            

        }
        else if (_LastStandingOn != null)
            _LastStandingOn.SendMessage("ControllerExit", this, SendMessageOptions.DontRequireReceiver);
        _LastStandingOn = null;


        Debug.DrawLine(transform.position, _ActiveGlobalPlatformPoint);

        _Transform.Translate(DeltaMovement, Space.World);
    }

    private void HandlePlatforms()
    {
       

        
        if (standingOn != null)
        {
            var newGlobalPlatformPoint = standingOn.transform.TransformPoint(_ActiveLocalPlatformPoint);
            var moveDistance = newGlobalPlatformPoint - _ActiveGlobalPlatformPoint;

            if (moveDistance != Vector3.zero)
                transform.Translate(moveDistance, Space.World);
            platformVelocity = (newGlobalPlatformPoint - _ActiveGlobalPlatformPoint) / Time.deltaTime;
        }
        else
            platformVelocity = Vector3.zero;

        standingOn = null;
        
        

    }

    private void CorrectHorizontalPlacement(ref Vector3 deltaMovement, bool isRight)
    {
        
        var halfWidth = (_boxCollider.size.x * _LocalScale.x ) / 2f;
        var rayorigin = isRight ? _RaycastBottomRight : _RaycastBottomLeft;
        if (isRight)
            rayorigin.x -= (halfWidth - skinWidth);
        else
            rayorigin.x += (halfWidth - skinWidth);

        var rayDirection = isRight ? Vector2.right : -Vector2.right;
        var offset = 0f;

        for (var i = 1; i < totalHorizontalRays - 1; i++)
        {
            var rayVector = new Vector2(deltaMovement.x + rayorigin.x, deltaMovement.y + deltaMovement.y + (i * _VerticalRayDistance));
            //Debug.DrawRay(rayVector, rayDirection * halfWidth, isRight ? Color.cyan : Color.magenta);

            var raycastHit = Physics2D.Raycast(rayVector, rayDirection, halfWidth, PlatformMask);
            if (!raycastHit)
                continue;
            offset = isRight ? ((raycastHit.point.x - _Transform.position.x) - halfWidth) : (halfWidth - (_Transform.position.x - raycastHit.point.x));


        }
        deltaMovement.x += offset;
 

    }

    private void CalculateRayOrigins()
    {
        var box = new Rect(_boxCollider.bounds.min.x,
                     _boxCollider.bounds.min.y,
                     _boxCollider.bounds.size.x,
                     _boxCollider.bounds.size.y
                       );

        var bcSize = new Vector2(_boxCollider.size.x * Mathf.Abs(_LocalScale.x), _boxCollider.size.y * Mathf.Abs(_LocalScale.y) / 2);
        var center = new Vector2(_boxCollider.center.x * _LocalScale.x, _boxCollider.center.y * _LocalScale.y);

        _RaycastTopLeft = _Transform.position + new Vector3(center.x - bcSize.x + skinWidth, center.y + bcSize.y - skinWidth);
        _RaycastBottomLeft = _Transform.position + new Vector3(center.x - bcSize.x + skinWidth, center.y - bcSize.y + skinWidth);

        _RaycastBottomRight = _Transform.position + new Vector3(center.x + bcSize.x - skinWidth, center.y - bcSize.y + skinWidth);

        
    }

    private void MoveHorizontially(ref Vector3 DeltaMovement)
    {

       

        //Debug.Log("NO");
        var isGoingRight = DeltaMovement.x > 0;
        var rayDistance = Mathf.Abs(DeltaMovement.x) + skinWidth;
        var rayDirection = isGoingRight ? Vector2.right : -Vector2.right;
        var rayOrigin = isGoingRight ? _RaycastBottomRight : _RaycastBottomLeft;

        for (var i = 0; i < totalHorizontalRays; i++)
        {
           
            var rayVector = new Vector2(rayOrigin.x, rayOrigin.y + (i * _VerticalRayDistance));
            Debug.DrawRay(rayVector, rayDirection * rayDistance, Color.yellow);

            if (!(Physics.Raycast(rayVector, rayDirection, out HitInfo, rayDistance, PlatformMask)))
                continue;


            if (i == 0 && HandleHorizontalSlope(ref DeltaMovement, Vector2.Angle(HitInfo.normal, Vector2.up), isGoingRight))
                break;

            DeltaMovement.x = HitInfo.point.x - rayVector.x;
            rayDistance = Mathf.Abs(DeltaMovement.x);

            if (isGoingRight)
            {
                DeltaMovement.x -= skinWidth;
                State.isCollidingRight = true;
            }
            else
            {
                DeltaMovement.x += skinWidth;
                State.isCollidingLeft = true;
            }

            if (rayDistance < skinWidth + .0001f)
                break;


        }
    }

    private void MoveVertically(ref Vector3 DeltaMovement)
    {
        var box = new Rect(_boxCollider.bounds.min.x,
                 _boxCollider.bounds.min.y,
                 _boxCollider.bounds.size.x,
                 _boxCollider.bounds.size.y
                   );

        var isGoingUp = DeltaMovement.y > 0;
        var rayDistance = Mathf.Abs(DeltaMovement.y) + skinWidth;
        var rayDirection = isGoingUp ? Vector2.up : -Vector2.up;
        // var rayOrigin = isGoingUp ? _RaycastTopLeft : _RaycastBottomLeft;
        var rayOrigin = isGoingUp ? new Vector3(box.xMin, box.yMax, 0) : new Vector3(box.xMin, box.yMin, 0);

        rayOrigin.x += DeltaMovement.x;

        var standingOnDistance = float.MaxValue;

        var startPoint = new Vector3(box.xMin, box.yMin, transform.position.z);
        Debug.DrawRay(startPoint, -Vector3.up, Color.yellow);

        for (var i = 0; i < totalVerticalRays; i++)
        {
            var rayVector = new Vector3(rayOrigin.x + (i * _HorizontalRayDistance), rayOrigin.y, transform.position.z);

            Debug.DrawRay(rayVector, rayDirection * rayDistance, Color.red);

            //  Physics.Raycast(rayVector, rayDirection, out HitInfo, rayDistance, PlatformMask);

            if (!(Physics.Raycast(rayVector, rayDirection, out HitInfo, rayDistance, PlatformMask)))
                continue;


            if (!isGoingUp)
            {
                var verticalDistancetoHit =transform.position.y - HitInfo.point.y;
                if (verticalDistancetoHit < standingOnDistance)
                {
                    standingOnDistance = verticalDistancetoHit;
                    standingOn = HitInfo.collider.gameObject;

                }
            }
            DeltaMovement.y = HitInfo.point.y - rayVector.y;
            rayDistance = Mathf.Abs(DeltaMovement.y);

            if (isGoingUp)
            {

                DeltaMovement.y -= skinWidth;
                State.isCollidingAbove = true;

            }
            else
            {
                DeltaMovement.y += skinWidth;
                State.isCollidingBelow = true;

            }

            if (!isGoingUp && DeltaMovement.y > .0001f)
            {
                State.isMovingUpSlope = true;
            }
            if (rayDistance < skinWidth + .0001f)
                break;
        }

    }

    private void HandleVerticalSlope(ref Vector3 DeltaMovement)
    {
       var center = (_RaycastBottomLeft.x + _RaycastBottomRight.x) / 2;
       var direction = -Vector2.up;
        var slopeDistance = slopeLimitTangant *(_RaycastBottomRight.x - center);
        var slopeRayVector = new Vector2(center, _RaycastBottomLeft.y);
        Debug.DrawRay(slopeRayVector, direction * slopeDistance, Color.white);
        var raycastHit = Physics2D.Raycast(slopeRayVector, direction, slopeDistance, PlatformMask);

        if (!raycastHit)
            return;

        var isMovingDownSlope = Mathf.Sign(raycastHit.normal.x) == Mathf.Sign(DeltaMovement.x);
        if (!isMovingDownSlope)
            return;

        var angle = Vector2.Angle(raycastHit.normal, Vector2.up);
        if (Mathf.Abs(angle) < .0001f)
        {
            State.isMovingDownSlope = false;
            return;
        }

        State.isMovingDownSlope = true;
        State.slopeAngle = angle;
        DeltaMovement.y = raycastHit.point.y - slopeRayVector.y;


    }

    private bool HandleHorizontalSlope(ref Vector3 DeltaMovement, float angle, bool isGoingRight)
    {
        if (Mathf.RoundToInt(angle) == 90)
            return false;

        if (angle > Parameters.slopeLimit)
        {
            DeltaMovement.x = 0;
            return true;
        }

        if (DeltaMovement.y > .07f)
            return true;

        DeltaMovement.x += isGoingRight ? -skinWidth : skinWidth;
        DeltaMovement.y = Mathf.Abs(Mathf.Tan(angle * Mathf.Deg2Rad) * DeltaMovement.x);
        State.isMovingUpSlope = true;
        State.isCollidingBelow = true;
        return true;

    }
}
