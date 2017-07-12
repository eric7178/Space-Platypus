using UnityEngine;


public class From_WorldPoint_Text_Positioner : IFloating_Text_Positioner 
{

    private readonly Camera _camera;
    private readonly Vector3 _WorldPos;
    private float _ttl;
    private readonly float _speed;
    private float _yOffset;

    public From_WorldPoint_Text_Positioner(Camera camera, Vector3 WorldPos, float ttl, float speed)
    {
        _camera = camera;
        _WorldPos = WorldPos;
        _ttl = ttl;
        _speed = speed;
    }
	
    public bool GetPosition(ref Vector2 position, GUIContent content, Vector2 Size)
    {
        if((_ttl -= Time.deltaTime) <= 0)
        return false;

        var screenpos = _camera.WorldToScreenPoint(_WorldPos);
        position.x = screenpos.x - (Size.x / 2);
        position.y = Screen.height - screenpos.y - _yOffset;
        _yOffset += Time.deltaTime * _speed;

        return true;
    }
}
