using UnityEngine;


public class Centered_Text_Positioner : IFloating_Text_Positioner
{
    private readonly float _Speed;
    private float _textPos;


    public Centered_Text_Positioner(float speed)
    {
        _Speed = speed;

    }

    public bool GetPosition(ref Vector2 position, GUIContent content, Vector2 size)
    {
        _textPos += Time.deltaTime * _Speed;

        if (_textPos > 1)
            return false;

        position = new Vector2(Screen.width /2f - size.x / 2f, Mathf.Lerp(Screen.height / 2f + size.y, 0, _textPos));
        return true;

    }
	
}
