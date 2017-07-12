using System;
using UnityEngine;
using System.Collections;
[Serializable]
public class Controller_Parameters_2D
{
    public enum jumpBehavior {canJumpGround, canJumpAnywhere,cantJump}
    public jumpBehavior jumpRestrictions;
    public Vector2 maxVelocity = new Vector2(float.MaxValue,float.MaxValue);
    [Range (0,180)]
    public float slopeLimit = 30f;
    public float gravity = -25f;
    public float jumpFrequentcy = .25f;
    public float jumpMagnitude = 12;




}
