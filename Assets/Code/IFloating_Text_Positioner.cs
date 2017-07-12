using UnityEngine;
using System.Collections;

public interface IFloating_Text_Positioner 
{
    bool GetPosition(ref Vector2 position, GUIContent content, Vector2 size);
    
}
