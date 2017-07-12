using UnityEngine;
using System.Collections;

public class Controller_State_2D
{
    public bool isCollidingLeft  { get; set; }
    public bool isCollidingRight { get; set; }
    public bool isCollidingAbove { get; set; }
    public bool isCollidingBelow { get; set; }
    public bool isMovingDownSlope { get; set; }
    public bool isMovingUpSlope { get; set; }
    public bool isGrounded { get { return isCollidingBelow; } }
    public float slopeAngle { get; set; }

    public bool hasCollisions { get { return isCollidingLeft || isCollidingRight || isCollidingAbove || isCollidingBelow; } }

    public void Reset()
    {
        isCollidingLeft = false;
        isCollidingRight = false;
        isCollidingAbove = false;
        isCollidingBelow = false;

        slopeAngle = 0;

    }

    public override string ToString()
    {
        return string.Format("(Controller: r:{0} l:{1} a:{2} b:{3} down-slope: {4} up-Slope: {5} angle:{6})",
            isCollidingLeft,
            isCollidingRight,
            isCollidingAbove,
            isCollidingBelow,
            isMovingDownSlope,
            isMovingUpSlope,
            slopeAngle);
    }

}
