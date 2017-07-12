using UnityEngine;
using System.Collections;

public class Jump_Platform : MonoBehaviour 
{

    public float jumpMagnitude = 20;

    public void ControllerEnter(Character_Controller_2D controller)
    {
        controller.SetVerticalForce(jumpMagnitude);
        

    }
}
